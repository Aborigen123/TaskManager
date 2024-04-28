using Service.DTOs;
using Service.ResponseImpl;
using System.Security.Claims;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<MethodResult<List<UserDTO>>> GetUsersAsync();
        Task<MethodResult<string>> RegisterAsync(RegisterDTO registerDto);
        Task<MethodResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDto);
    }
}
