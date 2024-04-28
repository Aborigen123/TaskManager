using System.ComponentModel.DataAnnotations;

namespace Service.DTOs
{
    public class TaskItemRequestDTO
    {
        [Required(ErrorMessage = "Description cannot be empty"),
         MaxLength(450, ErrorMessage = "The length of the text cannot be greater than 450")]
        public string Description { get; set; }
    }
}
