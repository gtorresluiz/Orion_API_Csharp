using SprintCsharp.Domain.Entities;
using SprintCsharp.Infra.Repositories;

namespace SprintCsharp.Application.Services;

public class UserService
{
    private readonly UserRepository _repo;

    public UserService(UserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<User>> ListAsync() => await _repo.GetAllAsync();

    public async Task<User?> GetAsync(int id) => await _repo.GetByIdAsync(id);

    /// <summary>Cria usuário. Retorna o usuário criado ou null em caso de validação falha.</summary>
    public async Task<(bool Success, string Message, User? Created)> CreateAsync(string name, string email, string balanceInput, InvestmentType investment, ProfessionLevel level)
    {
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Nome é obrigatório.", null);

        if (!User.IsValidEmail(email))
            return (false, "E-mail inválido.", null);

        if (!decimal.TryParse(balanceInput, out var balance))
            return (false, "Saldo inválido. Informe um número (ex: 1000.50).", null);

        var user = new User
        {
            Name = name.Trim(),
            Email = email.Trim(),
            Balance = balance,
            PreferredInvestment = investment,
            Level = level,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(user);
        return (true, "Usuário criado com sucesso.", created);
    }

    /// <summary>Atualiza usuário existente. Retorna true se atualizado.</summary>
    public async Task<(bool Success, string Message)> UpdateAsync(int id, string name, string email, string balanceInput, InvestmentType investment, ProfessionLevel level)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return (false, "Usuário não encontrado.");

        if (string.IsNullOrWhiteSpace(name))
            return (false, "Nome é obrigatório.");

        if (!User.IsValidEmail(email))
            return (false, "E-mail inválido.");

        if (!decimal.TryParse(balanceInput, out var balance))
            return (false, "Saldo inválido. Informe um número (ex: 1000.50).");

        user.Name = name.Trim();
        user.Email = email.Trim();
        user.Balance = balance;
        user.PreferredInvestment = investment;
        user.Level = level;
        user.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(user);
        return (true, "Usuário atualizado com sucesso.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return (false, "Usuário não encontrado.");

        await _repo.DeleteAsync(user);
        return (true, "Usuário deletado com sucesso.");
    }

    public async Task<(bool Success, string Message)> DepositAsync(int id, string amountInput)
    {
        if (!decimal.TryParse(amountInput, out var amount) || amount <= 0)
            return (false, "Valor de depósito inválido. Deve ser número positivo.");

        var user = await _repo.GetByIdAsync(id);
        if (user == null) return (false, "Usuário não encontrado.");

        user.Balance += amount;
        user.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(user);
        return (true, "Depósito efetuado.");
    }

    public async Task<(bool Success, string Message)> WithdrawAsync(int id, string amountInput)
    {
        if (!decimal.TryParse(amountInput, out var amount) || amount <= 0)
            return (false, "Valor de saque inválido. Deve ser número positivo.");

        var user = await _repo.GetByIdAsync(id);
        if (user == null) return (false, "Usuário não encontrado.");

        if (user.Balance < amount) return (false, "Saldo insuficiente.");

        user.Balance -= amount;
        user.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(user);
        return (true, "Saque efetuado.");
    }
}
