using DataLayer.DatabaseEntities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.DatabaseEntities
{
    public class TaskItem : BaseEntity
    {
        [Required, MaxLength(450)]
        public string Description { get; set; }
        public TaskItemState State { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public List<AssignmentHistory> AssignmentHistories { get; set; } = new();
    }
}
