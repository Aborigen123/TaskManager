using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.ExtensionMethods;
using Service.Interfaces;

namespace InventorSoftTestTask.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="taskDto">Data about new task</param>
        /// <returns>A newly created task.</returns>
        /// <response code="200">Return information about the newly created task</response>
        /// <response code="404">User not found</response>
        [HttpPost("CreateTask")]
        public async Task<ActionResult<BaseTaskItemResponseDTO>> CreateTaskAsync([FromBody] TaskItemRequestDTO taskDto)
        {
            var result = await _taskService.CreateTaskAsync(taskDto, User.Claims.GetUserName());

            return result.DecideWhatToReturn();
        }

        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns>Tasks information</returns>
        /// <response code="200">Information about all tasks, assignment history, and users who were assigned to the tasks</response>
        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<List<TaskItemResponseDTO>>> GetAllTasksAsync()
        {
            var result = await _taskService.GetAllTasksAsync();

            return result.DecideWhatToReturn();
        }
    }
}