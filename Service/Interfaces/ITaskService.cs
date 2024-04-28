using Service.DTOs;
using Service.ResponseImpl;

namespace Service.Interfaces
{
    public interface ITaskService
    {
        Task<MethodResult<BaseTaskItemResponseDTO>> CreateTaskAsync(TaskItemRequestDTO taskDto, string userName);
        Task<MethodResult<List<TaskItemResponseDTO>>> GetAllTasksAsync();
    }
}