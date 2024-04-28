using System.ComponentModel.DataAnnotations;

namespace Service.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "User name cannot be empty")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
