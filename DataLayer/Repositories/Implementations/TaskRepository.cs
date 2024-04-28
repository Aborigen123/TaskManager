using DataLayer.DatabaseEntities;
using DataLayer.DatabaseEntities.Enums;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Implementations
{
    public class TaskRepository : BaseRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsEnoughTasksForReassigne()
        {
            return await GetAll().Skip(1).AnyAsync();
        }

        public async Task<List<TaskItem>> GetTaskItemsWithAssignmentHistory(TaskItemState state)
        {
            return await GetAll(u => u.State == state)
                .Include(a => a.AssignmentHistories)
                .ToListAsync();
        }
    }
}
