using GP_API.MiddleWares;
using GP_API.Models;
using GP_API.Repository;
using GP_API.Repository.Interfaces;
using GP_API.Services;
using GP_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appSettings.{EnvironmentName.Development}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

//DI 
{
    var services = builder.Services;
    services.AddControllers();
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
    services.AddScoped<IUserRepository, UsersRepository>();
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IUserService, UserService>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
{
    app.UseHttpsRedirection();

    app.UseMiddleware<JwtMiddleWare>();

    app.UseAuthorization();

    app.MapControllers();
}


app.Run();
