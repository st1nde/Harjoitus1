using Harjoitus1.Models;

namespace Harjoitus1.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(string username);

        Task<User> GetUserAsync(long id);

        Task<User> NewUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(User user);

    }
}
