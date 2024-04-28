using DataLayer.DatabaseEntities;
using DataLayer.DatabaseEntities.Enums;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class IdentitySeed
    {
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentHistoryRepository _assignmentHistoryRepository;
        private readonly ITaskRepository _taskRepository;

        public IdentitySeed(IUserRepository userRepository, IAssignmentHistoryRepository assignmentHistoryRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _assignmentHistoryRepository = assignmentHistoryRepository;
            _taskRepository = taskRepository;
        }

        public async Task SeedBaseRecords()
        {
            var firstUser = _userRepository.Add(new User { Name = "Name1", Password = BCrypt.Net.BCrypt.HashPassword("Name1") });
            var secondUser = _userRepository.Add(new User { Name = "Name2", Password = BCrypt.Net.BCrypt.HashPassword("Name2") });
            _userRepository.Add(new User { Name = "Name3", Password = BCrypt.Net.BCrypt.HashPassword("Name3") });

            var firstTask = _taskRepository.Add(new TaskItem() { Description = "1", State = TaskItemState.InProgress, Author = firstUser });
            _taskRepository.Add(new TaskItem() { Description = "2", State = TaskItemState.Waiting, Author = secondUser });


            _assignmentHistoryRepository.Add(new AssignmentHistory() { IsCurrent = true, Task = firstTask, AssignedUser = firstUser });

            await _userRepository.CommitAsync();

            var users = await _userRepository.GetAll().ToListAsync();
        }
    }
}
