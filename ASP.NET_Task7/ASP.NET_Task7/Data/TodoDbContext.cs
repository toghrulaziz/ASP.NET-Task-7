using ASP.NET_Task7.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7.Data
{
    public class TodoDbContext : IdentityDbContext<AppUser>
    {
        public TodoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
    }
}
