using ASP.NET_Task7.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}
