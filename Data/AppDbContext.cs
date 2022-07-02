using Microsoft.EntityFrameworkCore;
using MyMinimalAPI.Models;

namespace MyMinimalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Command> Commands => Set<Command>();
    }
}