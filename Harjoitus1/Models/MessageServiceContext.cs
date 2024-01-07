using Microsoft.EntityFrameworkCore;

namespace Harjoitus1.Models
{
    public class MessageServiceContext:DbContext
    {
        public MessageServiceContext(DbContextOptions<MessageServiceContext>options) : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;


    }
}
