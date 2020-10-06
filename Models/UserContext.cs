using Microsoft.EntityFrameworkCore;

namespace Bug_Tracker_Api.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }

        public DbSet<User> UserItems { get; set; }
    }
}