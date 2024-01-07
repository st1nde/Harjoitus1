using Harjoitus1.Models;
using Harjoitus1.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Harjoitus1.Middleware
{

    public interface IUserAuthenticationService
    {
        Task<User> Authenticate(string username, string password);

        User CreateUserCredentials(User user);
        Task<bool> IsMyMessage(string value, int id);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserRepository _repository;
        private readonly IMessageRepository _messageRepository;

        public UserAuthenticationService(IUserRepository repository, IMessageRepository messageRepository)
        {
            _repository = repository;
            _messageRepository = messageRepository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            User? user;
            user = await _repository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }



            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));



            if (hashedPassword != user.Password)
            {
                return null;
            }
            return user;
        }

        public User CreateUserCredentials(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));

            user.Password = hashedPassword;
            user.Salt = salt;
            user.joinDate =  user.joinDate != null ? user.joinDate : DateTime.Now;
            user.lastLogin = DateTime.Now;

            return user;
        }

        public async Task<bool> IsMyMessage(string username, int messageId)
        {
            User? user = await _repository.GetUserAsync(username);
            if (user == null)
            {
                return false;
            }
            Message? message = await _messageRepository.GetMessageAsync(messageId);
            if (message == null)
            {
                return false;
            }
            if (message.Sender == user)
            {
                return true;
            }
            return false;
        }
    }
}
