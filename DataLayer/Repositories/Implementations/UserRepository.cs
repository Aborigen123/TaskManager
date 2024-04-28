using DataLayer.DatabaseEntities;
using DataLayer.DatabaseEntities.Enums;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetAvailableUser()
        {
            return await GetAll()
                .FirstOrDefaultAsync(u => !u.AssignmentHistories.Any(a => a.IsCurrent && a.Task.State == TaskItemState.InProgress));
        }

        public async Task<List<User>> GetNotAvailableUser(List<User> users)
        {
            return await GetAll(u => users.Contains(u)).AsNoTracking()
                    .Where(u => !u.AssignmentHistories.Any(a => a.IsCurrent && a.Task.State == TaskItemState.InProgress))
                    .ToListAsync();
        }

        public async Task<bool> IsEnoughUsersForReassigneTasks()
        {
            return await GetAll().Skip(1).AnyAsync();
        }
    }
}
