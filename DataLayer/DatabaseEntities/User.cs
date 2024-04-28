using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.DatabaseEntities
{
    [Index(nameof(Name), IsUnique = true)]
    public class User : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
        public List<TaskItem> Tasks { get; set; } = new ();
        public List<AssignmentHistory> AssignmentHistories { get; set; } = new();
    }
}
