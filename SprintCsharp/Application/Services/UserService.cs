using System.Text.Json;
using SprintCsharp.Application.Interfaces;
using SprintCsharp.Domain.Entities;

namespace SprintCsharp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    public async Task<User> CreateAsync(string name, string email, decimal initialBalance,
        InvestmentType preferred, ProfessionLevel level)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome inválido");
        if (!email.Contains("@")) throw new ArgumentException("Email inválido");
        var user = new User
        {
            Name = name.Trim(),
            Email = email.Trim(),
            Balance = initialBalance,
            PreferredInvestment = preferred,
            Level = level
        };
        return await _repo.AddAsync(user);
    }

    public Task<IEnumerable<User>> GetAllAsync() => _repo.GetAllAsync();
    public Task<User?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task UpdateAsync(User user) => _repo.UpdateAsync(user);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

    public async Task DepositAsync(int id, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Valor deve ser maior que zero");
        var u = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Usuário não encontrado");
        u.Balance += amount;
        await _repo.UpdateAsync(u);
    }

    public async Task WithdrawAsync(int id, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Valor deve ser maior que zero");
        var u = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Usuário não encontrado");
        if (u.Balance < amount) throw new InvalidOperationException("Saldo insuficiente");
        u.Balance -= amount;
        await _repo.UpdateAsync(u);
    }

    public async Task ExportToJsonAsync(string path)
    {
        var users = await _repo.GetAllAsync();
        var opts = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(users, opts);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task ImportFromJsonAsync(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        var json = await File.ReadAllTextAsync(path);
        var users = JsonSerializer.Deserialize<List<User>>(json);
        if (users == null) return;
        foreach (var u in users)
        {
            u.Id = 0; // força insert
            await _repo.AddAsync(u);
        }
    }

    public async Task ExportToTxtAsync(string path)
    {
        var users = await _repo.GetAllAsync();
        var lines = users.Select(u => $"{u.Id};{u.Name};{u.Email};{u.Balance};{u.PreferredInvestment};{u.Level}");
        await File.WriteAllLinesAsync(path, lines);
    }
}
