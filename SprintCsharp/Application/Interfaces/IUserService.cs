using SprintCsharp.Domain.Entities;

namespace SprintCsharp.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateAsync(string name, string email, decimal initialBalance,
                           InvestmentType preferred, ProfessionLevel level);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task DepositAsync(int id, decimal amount);
    Task WithdrawAsync(int id, decimal amount);
    Task ExportToJsonAsync(string path);
    Task ImportFromJsonAsync(string path);
    Task ExportToTxtAsync(string path);
}
