namespace Service.DTOs
{
    public class TaskItemResponseDTO : BaseTaskItemResponseDTO
    {
        public List<AssignmentHistoryDTO> AssignmentHistories { get; set; } = new();
        public UserDTO? Author { get; set; }
        public UserDTO? AssignedUser { get; set; }
    }
}
