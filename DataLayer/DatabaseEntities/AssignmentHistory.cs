namespace DataLayer.DatabaseEntities
{
    public class AssignmentHistory : BaseEntity
    {
        public bool IsCurrent { get; set; }
        public int AssignedUserId { get; set; }
        public int TaskId { get; set; }
        public User AssignedUser { get; set; }
        public TaskItem Task { get; set; }
    }
}
