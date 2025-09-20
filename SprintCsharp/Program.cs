using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SprintCsharp.Application.Interfaces;
using SprintCsharp.Application.Services;
using SprintCsharp.Infra.Data;
using SprintCsharp.Infra.Repositories;
using SprintCsharp.UI;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, cfg) => cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseSqlServer(ctx.Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IUserService, UserService>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var svc = scope.ServiceProvider.GetRequiredService<IUserService>();
await Menu.RunAsync(svc);
