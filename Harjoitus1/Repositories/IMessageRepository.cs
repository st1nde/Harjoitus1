using Harjoitus1.Models;

namespace Harjoitus1.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();

        Task<IEnumerable<Message>> SearchMessages(string searchtext);

        Task<IEnumerable<Message>> GetSentMessagesAsync(User user);

        Task<IEnumerable<Message>> GetReceivedMessagesAsync(User user);

        Task<Message?> GetMessageAsync(long id);

        Task<Message> NewMessageAsync(Message message);

        Task<bool> UpdateMessageAsync(Message message);

        Task<bool> DeleteMessageAsync(Message message);
   
    }
}
