using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Interfaces;

namespace InventorSoftTestTask.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registration new user
        /// </summary>
        /// <param name="registerDto">Message about successfully registered</param>
        /// <response code="200">Message about successfully registered</response>
        /// <response code="400">User with same name already exist</response>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDto)
        {
            var result = await _userService.RegisterAsync(registerDto);

            return result.DecideWhatToReturn();
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="login">Data about user for login</param>
        /// <returns>JWT token</returns>
        /// <response code="200">JWT token</response>
        /// <response code="400">Password not correct</response>
        /// <response code="404">User not found</response>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> LoginAsync([FromBody] LoginRequestDTO login)
        {
            var result = await _userService.LoginAsync(login);

            return result.DecideWhatToReturn();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>All registrated users</returns>
        /// <response code="200">Return all registrated users</response>
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetUsersAsync()
        {
            var result = await _userService.GetUsersAsync();

            return result.DecideWhatToReturn();
        }
    }
}