using DataLayer.DatabaseEntities;
using DataLayer.DatabaseEntities.Enums;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.ExtensionMethods;

namespace Service.Implementations.ScheduleJobs
{
    public class ReassigneTasksJob
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public ReassigneTasksJob(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task ExecuteAsync()
        {
            Console.WriteLine("Reassigne tasks has started working " + System.DateTime.UtcNow.TimeOfDay);

            if (await _userRepository.IsEnoughUsersForReassigneTasks() && await _taskRepository.IsEnoughTasksForReassigne())
            {
                // First, the ReassignInProgressTasks will reassign 'InProgress' tasks among users, and check which tasks can be moved to 'Completed' status.
                // It will also identify users without available tasks (those who do not have any tasks in progress).
                // Additionally, it will prevent tasks from being reassigned to users who have previously been assigned to them.
                var availableUsers = await ReassignInProgressTasks();
                if (availableUsers.Any())
                {
                    // Tasks in waiting status will assign to available users
                    AssignWaitingTasks(availableUsers);
                }
            }

        }

        public async Task<List<User>> ReassignInProgressTasks()
        {
            var rand = new Random();
            var activeTasks = await _taskRepository.GetTaskItemsWithAssignmentHistory(TaskItemState.InProgress);
            var users = await _userRepository.GetAll().AsNoTracking().ToListAsync();

            foreach (var activeTask in activeTasks)
            {
                var notAssignedUsers = users.Where(u => !activeTask.AssignmentHistories.Any(ah => ah.AssignedUserId == u.Id)).ToList();

                if (!notAssignedUsers.Any() && activeTask.AssignmentHistories.Count >= 3)
                {
                    activeTask.State = TaskItemState.Completed;
                    _taskRepository.Update(activeTask);
                }
                else if (notAssignedUsers.Any())
                {
                    var newAssignUser = notAssignedUsers[rand.Next(notAssignedUsers.Count)];
                    activeTask.AssignmentHistories.GetCurrentAssignedHistory().IsCurrent = false;
                    activeTask.AssignmentHistories.Add(new AssignmentHistory()
                    {
                        IsCurrent = true,
                        AssignedUserId = newAssignUser.Id,
                        Task = activeTask,
                    });

                    _taskRepository.Update(activeTask);
                    users.Remove(newAssignUser);
                }
                else
                {
                    return await _userRepository.GetNotAvailableUser(users);
                }

                await _userRepository.CommitAsync();
            }

            return await _userRepository.GetNotAvailableUser(users);
        }

        public async void AssignWaitingTasks(List<User> notAvailableUser)
        {
            var awaitingTasks = await _taskRepository.GetTaskItemsWithAssignmentHistory(TaskItemState.Waiting);

            foreach (var awaitingTask in awaitingTasks)
            {
                if (notAvailableUser.Any())
                {
                    awaitingTask.State = TaskItemState.InProgress;
                    if (awaitingTask.AssignmentHistories.Any(a => a.IsCurrent))
                    {
                        awaitingTask.AssignmentHistories.GetCurrentAssignedHistory().IsCurrent = false;
                    }

                    awaitingTask.AssignmentHistories.Add(new AssignmentHistory()
                    {
                        IsCurrent = true,
                        AssignedUserId = notAvailableUser.First().Id
                    });

                    notAvailableUser.Remove(notAvailableUser.First());
                    _taskRepository.Update(awaitingTask);

                    await _taskRepository.CommitAsync();
                }
                else
                {
                    break;
                }
            }
        }
    }
}