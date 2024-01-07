using Harjoitus1.Middleware;
using Harjoitus1.Models;
using Harjoitus1.Repositories;
using Harjoitus1.Middleware;

namespace Harjoitus1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserAuthenticationService _authenticationService;

        public UserService(IUserRepository repository, IUserAuthenticationService authenticationService)
        {
            _repository = repository;
            _authenticationService = authenticationService;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            User? user = await _repository.GetUserAsync(id);
            if (user != null)
            {
                return await _repository.DeleteUserAsync(user);
            }
            return false;
        }

        public async Task<UserDTO> GetUserAsync(long id)
        {
            User? user = await _repository.GetUserAsync(id);
            if (user != null)
            {
                return null;
            }
            return UserToDTO(user);
        }

        public  Task<UserDTO> GetUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            IEnumerable<User> users = await _repository.GetUsersAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach (User user in users)
            {
                result.Add(UserToDTO(user));
            }
            return result;
        }

        public async Task<UserDTO?> NewUserAsync(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.userName);
            if (dbUser != null)
            {
                return null;
            }


            User? newUser = _authenticationService.CreateUserCredentials(user);
            if(newUser != null)
            {
                return UserToDTO(await _repository.NewUserAsync(user));
            }
            return null;

            
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            User dbUser = await _repository.GetUserAsync(user.userName);
            if (dbUser == null)
            {
                return false;
            }
            dbUser.firstName = user.firstName;
            dbUser.lastName = user.lastName;
            dbUser.Password = user.Password;
            dbUser = _authenticationService.CreateUserCredentials(dbUser);

            return await _repository.UpdateUserAsync(dbUser);
        }

        private UserDTO UserToDTO(User user)
        {
            UserDTO dto = new UserDTO();
            dto.userName = user.userName;
            dto.firstName = user.firstName;
            dto.lastName = user.lastName;
            dto.joinDate = user.joinDate;
            dto.lastLogin = user.lastLogin;

            return dto;
        }

    }
}
