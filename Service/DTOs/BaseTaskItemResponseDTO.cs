using DataLayer.DatabaseEntities.Enums;

namespace Service.DTOs
{
    public class BaseTaskItemResponseDTO
    {
        public string Description { get; set; }
        public TaskItemState State { get; set; }
        public int AuthorId { get; set; }
        public int? AssignedId { get; set; }
    }
}
