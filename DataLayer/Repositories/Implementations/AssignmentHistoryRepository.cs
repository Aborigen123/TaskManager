using DataLayer.DatabaseEntities;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories.Implementations
{
    public class AssignmentHistoryRepository : BaseRepository<AssignmentHistory>, IAssignmentHistoryRepository
    {
        public AssignmentHistoryRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}
