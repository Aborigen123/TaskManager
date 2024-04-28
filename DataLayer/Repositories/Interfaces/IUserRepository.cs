using DataLayer.DatabaseEntities;

namespace DataLayer.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetAvailableUser();
        Task<List<User>> GetNotAvailableUser(List<User> users);
        Task<bool> IsEnoughUsersForReassigneTasks();
    }
}
