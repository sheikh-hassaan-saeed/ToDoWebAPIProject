using Microsoft.EntityFrameworkCore;

namespace ToDoWebAPIProject.Models


{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
    }

}

