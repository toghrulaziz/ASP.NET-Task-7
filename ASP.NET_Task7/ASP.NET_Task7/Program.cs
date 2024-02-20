using ASP.NET_Task7;
using ASP.NET_Task7.Services.MailService;
using ASP.NET_Task7.Services.NatificationService;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwagger();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);


builder.Services.AddDomainServices(builder.Configuration);

builder.Services.AddTodoContext(builder.Configuration);

builder.Services.AddHostedService<TodoDeadlineNotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
