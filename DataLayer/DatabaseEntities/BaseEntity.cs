using System.ComponentModel.DataAnnotations;

namespace DataLayer.DatabaseEntities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
