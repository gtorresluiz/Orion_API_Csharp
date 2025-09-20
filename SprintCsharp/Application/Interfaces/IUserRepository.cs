using SprintCsharp.Domain.Entities;

namespace SprintCsharp.Application.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}
