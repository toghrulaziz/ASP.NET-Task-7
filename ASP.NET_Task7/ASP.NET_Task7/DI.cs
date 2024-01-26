using ASP.NET_Task7.Data;
using ASP.NET_Task7.Services.TodoServices;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7
{
    public static class DI
    {
        public static IServiceCollection AddTodoContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoDbContext>(op => op.UseSqlServer(configuration.GetConnectionString("TodoConStr")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            return services;
        }


        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoService, TodoService>();
            return services;
        }


    }
}
