using DataLayer.DatabaseEntities;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using Service.Interfaces;
using Service.ResponseImpl;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly MethodResultFactory _methodResultFactory;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(MethodResultFactory methodResultFactory, IUserRepository userRepository, IConfiguration configuration)
        {
            _methodResultFactory = methodResultFactory;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<MethodResult<string>> RegisterAsync(RegisterDTO registerDto)
        {
            var result = _methodResultFactory.Create<string>();

            if (_userRepository.Any(u => u.Name.Equals(registerDto.Username, StringComparison.OrdinalIgnoreCase)))
            {
                result.SetError("User with same name already exist", HttpStatusCode.BadRequest);
                return result;
            }

            _userRepository.Add(new User
            {
                Name = registerDto.Username,
                Password = HashPassword(registerDto.Password)
            });

            await _userRepository.CommitAsync();
            result.Data = "Successfully registered";

            return result;
        }

        public async Task<MethodResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDto)
        {
            var result = _methodResultFactory.Create<LoginResponseDTO>();

            var user = await _userRepository
                .GetAll(u => u.Name.Equals(loginRequestDto.Username, StringComparison.OrdinalIgnoreCase))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                result.SetError("User not found", HttpStatusCode.NotFound);
                return result;
            }

            if (!VerifyPassword(loginRequestDto.Password, user.Password))
            {
                result.SetError("Password not correct", HttpStatusCode.BadRequest);
                return result;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Secret"],
                audience: _configuration["JWT:Secret"],
                claims: new[] { new Claim(ClaimTypes.Name, user.Name) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            result.Data = new LoginResponseDTO
            {
                JWT = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return result;
        }

        public async Task<MethodResult<List<UserDTO>>> GetUsersAsync()
        {
            var result = _methodResultFactory.Create<List<UserDTO>>();

            result.Data = await _userRepository.GetAll().AsNoTracking()
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Password = u.Password
                })
                .ToListAsync();

            return result;
        }

        // BCrypt.Net auto made
        //1. Salt generation
        //2. Creating a Hash using Blowfish (password + salt)
        //3. Work Factor or cost parameter - hashing rounds (by default 10)
        //4. Saving Salt with Hash
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
