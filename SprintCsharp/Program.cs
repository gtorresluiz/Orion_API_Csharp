using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SprintCsharp.Application.Interfaces;
using SprintCsharp.Application.Services;
using SprintCsharp.Infra.Data;
using SprintCsharp.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Oracle connection string inline (como no projeto anterior)
var oracleConn = "User Id=RM98600;Password=091105;Data Source=oracle.fiap.com.br:1521/ORCL;";

// Registrar o DbContext usando Oracle (adicionar pacotes NuGet conforme abaixo)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(oracleConn)
           .LogTo(Console.WriteLine, LogLevel.Information) // debug opcional
);

// Registrar repositório e serviço para DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Log de diagnóstico de diretórios
Console.WriteLine($"ContentRootPath: {app.Environment.ContentRootPath}");
Console.WriteLine($"CurrentDirectory: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"AppBaseDirectory: {AppContext.BaseDirectory}");

// Serve arquivos estáticos (wwwroot)
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        ctx.Context.Response.Headers["Pragma"] = "no-cache";
        ctx.Context.Response.Headers["Expires"] = "0";
    }
});

// Mapeamento explícito para /swagger-ui -> wwwroot/swagger-ui (se existir)
var swaggerUiPhysicalPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "swagger-ui");
if (Directory.Exists(swaggerUiPhysicalPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(swaggerUiPhysicalPath),
        RequestPath = "/swagger-ui",
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            ctx.Context.Response.Headers["Pragma"] = "no-cache";
            ctx.Context.Response.Headers["Expires"] = "0";
        }
    });
}

// Candidate paths para custom.css (verifica vários locais comuns)
var candidatePaths = new[]
{
    Path.Combine(app.Environment.ContentRootPath, "wwwroot", "swagger-ui", "custom.css"),
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "swagger-ui", "custom.css"),
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot", "swagger-ui", "custom.css") // se executando em bin/<cfg>/netX
}.Select(p => Path.GetFullPath(p)).Distinct().ToArray();

// Log dos candidatos
Console.WriteLine("Caminhos candidatos para custom.css:");
foreach (var p in candidatePaths) Console.WriteLine($"  {p} (exists: {File.Exists(p)})");

// Endpoints de diagnóstico
app.MapGet("/debug/cssinfo", () =>
{
    var infos = candidatePaths.Select(p => new
    {
        path = p,
        exists = File.Exists(p),
        length = File.Exists(p) ? new FileInfo(p).Length : (long?)null,
        lastWriteUtc = File.Exists(p) ? new FileInfo(p).LastWriteTimeUtc : (DateTime?)null
    }).ToArray();
    return Results.Json(new
    {
        contentRoot = app.Environment.ContentRootPath,
        currentDirectory = Directory.GetCurrentDirectory(),
        appBase = AppContext.BaseDirectory,
        candidates = infos
    });
});

app.MapGet("/debug/customcss", async () =>
{
    var found = candidatePaths.FirstOrDefault(File.Exists);
    if (found == null) return Results.NotFound(new { error = "custom.css not found", candidates = candidatePaths });
    var text = await File.ReadAllTextAsync(found);
    Console.WriteLine($"/debug/customcss served from: {found} at {DateTime.UtcNow:O}");
    return Results.Text(text, "text/css");
});

// Registrar Swagger
var cssVersion = DateTime.UtcNow.Ticks;
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orión API v1");
    c.RoutePrefix = string.Empty;
    c.InjectStylesheet($"/swagger-ui/custom.css?v={cssVersion}");
    c.DocumentTitle = "💸 Orión - Plataforma de Investimentos";
});

app.UseAuthorization();
app.MapControllers();

app.Run();