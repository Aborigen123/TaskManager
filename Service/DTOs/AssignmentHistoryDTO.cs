namespace Service.DTOs
{
    public class AssignmentHistoryDTO
    {
        public int Id { get; set; }
        public bool IsCurrent { get; set; }
        public int AssignedUserId { get; set; }
        public int TaskId { get; set; }
    }
}
