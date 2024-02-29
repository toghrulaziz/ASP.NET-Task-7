using ASP.NET_Task7.Auth;
using ASP.NET_Task7.Configuration;
using ASP.NET_Task7.Data;
using ASP.NET_Task7.Models.Entities;
using ASP.NET_Task7.Providers;
using ASP.NET_Task7.Services.MailService;
using ASP.NET_Task7.Services.ProductService;
using ASP.NET_Task7.Services.RabbitMqService;
using ASP.NET_Task7.Services.TodoServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;

namespace ASP.NET_Task7
{
    public static class DI
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "My Api - V1",
                        Version = "v1",
                    });

                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Exapmle: \"Bearer {token}\""
                });

            });


            return services;
        }

        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>(setup => { })
                .AddEntityFrameworkStores<TodoDbContext>();

            var jwtConfig = new JwtConfig();
            configuration.GetSection("JWT").Bind(jwtConfig);
            services.AddSingleton(jwtConfig);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, setup =>
            {
                setup.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                };
            });

            services.AddAuthorization();

            return services;
        }


        public static IServiceCollection AddTodoContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoDbContext>(op => op.UseSqlServer(configuration.GetConnectionString("TodoConStr")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            return services;
        }


        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<IMailService>(provider => new MailService(
                    configuration["Smtp:Host"]!,
                    configuration.GetValue<int>("Smtp:Port"),
                    configuration["Smtp:Username"]!,
                    configuration["Smtp:Password"]!,
                    configuration["Smtp:FromAddress"]!
                )
            );
            services.AddScoped<IRequestUserProvider, RequestUserProvider>();

            var section = configuration.GetSection("RabbitMQ");
            var rabbitMQConfig = new RabbitMQConfiguration();
            section.Bind(rabbitMQConfig);
            services.AddSingleton(rabbitMQConfig);

            services.AddScoped<IConnectionFactory, ConnectionFactory>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitMQConfig.HostName,
                    UserName = rabbitMQConfig.UserName,
                    Password = rabbitMQConfig.Password,
                    Port = rabbitMQConfig.Port,
                };
                return factory;
            });
            services.AddScoped<IRabbitMQService, RabbitMQService>();
            return services;
        }


    }
}
