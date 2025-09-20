using SprintCsharp.Application.Services;
using SprintCsharp.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SprintCsharp.Infra.Data;
using SprintCsharp.UI;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        // 🔑 Connection string Oracle diretamente aqui
        var conn = "User Id=RM98600;Password=091105;Data Source=oracle.fiap.com.br:1521/ORCL;";

        services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(conn)
                   .LogTo(Console.WriteLine, LogLevel.Information) // debug opcional
        );

        // Repositório / Serviço / UI
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<ConsoleApp>();
    })
    .Build();

// executa app
using var scope = host.Services.CreateScope();
var app = scope.ServiceProvider.GetRequiredService<ConsoleApp>();
await app.RunAsync();
