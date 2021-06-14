using Microsoft.EntityFrameworkCore;
using TaskDataProvider.Model;

namespace TaskDataProvider
{
    public class TestDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=SERVER_NAME;Initial Catalog=DATABASE_NAME;");
        }
    }
}