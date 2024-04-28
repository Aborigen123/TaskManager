using DataLayer.DatabaseEntities;
using DataLayer.DatabaseEntities.Enums;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using Service.Interfaces;
using Service.ResponseImpl;
using System.Net;

namespace Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly MethodResultFactory _methodResultFactory;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(MethodResultFactory methodResultFactory, ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _methodResultFactory = methodResultFactory;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        //If we have an available user (a user who does not have any tasks in progress), we will assign one to him and set InProgress for new task
        //If we have an not available user (a user who have one or more tasks in progress), we will not assign to anyone and set Waiting status for new task
        public async Task<MethodResult<BaseTaskItemResponseDTO>> CreateTaskAsync(TaskItemRequestDTO taskDto, string userName)
        {
            var result = _methodResultFactory.Create<BaseTaskItemResponseDTO>();

            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                result.SetError("User not found", HttpStatusCode.NotFound);
                return result;
            }

            var availableUsers = await _userRepository.GetAvailableUser();
            var newTaskItem = new TaskItem()
            {
                Description = taskDto.Description,
                State = availableUsers is null ? TaskItemState.Waiting : TaskItemState.InProgress,
                Author = user
            };
            if (availableUsers != null)
            {
                newTaskItem.AssignmentHistories.Add(new AssignmentHistory()
                {
                    IsCurrent = true,
                    AssignedUser = availableUsers
                });
            }

            _taskRepository.Add(newTaskItem);
            await _taskRepository.CommitAsync();

            result.Data = new BaseTaskItemResponseDTO
            {
                Description = newTaskItem.Description,
                State = newTaskItem.State,
                AssignedId = availableUsers?.Id,
                AuthorId = user.Id
            };

            return result;
        }

        public async Task<MethodResult<List<TaskItemResponseDTO>>> GetAllTasksAsync()
        {
            var result = _methodResultFactory.Create<List<TaskItemResponseDTO>>();

            result.Data = await _taskRepository.GetAll().AsNoTracking()
                .Select(t => new TaskItemResponseDTO
                {
                    Description = t.Description,
                    State = t.State,
                    AuthorId = t.AuthorId,
                    AssignedId = t.AssignmentHistories.FirstOrDefault(a => a.IsCurrent).AssignedUserId,
                    Author = new UserDTO
                    {
                        Id = t.Author.Id,
                        Name = t.Author.Name
                    },
                    AssignedUser = t.AssignmentHistories.Where(a => a.IsCurrent).Select(a => new UserDTO
                    {
                        Id = t.Author.Id,
                        Name = t.Author.Name
                    })
                    .FirstOrDefault(),
                    AssignmentHistories = t.AssignmentHistories.Select(a => new AssignmentHistoryDTO
                    {
                        Id = a.Id,
                        IsCurrent = a.IsCurrent,
                        AssignedUserId = a.AssignedUserId,
                        TaskId = a.TaskId
                    }).ToList()
                })
                .ToListAsync();

            return result;
        }
    }
}
