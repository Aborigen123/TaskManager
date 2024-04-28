using DataLayer.DatabaseEntities;
using DataLayer.DatabaseEntities.Enums;

namespace DataLayer.Repositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<TaskItem>
    {
        Task<bool> IsEnoughTasksForReassigne();
        Task<List<TaskItem>> GetTaskItemsWithAssignmentHistory(TaskItemState state);
    }
}
