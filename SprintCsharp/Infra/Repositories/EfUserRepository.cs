using Microsoft.EntityFrameworkCore;
using SprintCsharp.Application.Interfaces;
using SprintCsharp.Domain.Entities;
using SprintCsharp.Infra.Data;

namespace SprintCsharp.Infra.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    public EfUserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<User> AddAsync(User user)
    {
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _ctx.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await _ctx.Users.FindAsync(id);

    public async Task UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var u = await _ctx.Users.FindAsync(id);
        if (u != null) { _ctx.Users.Remove(u); await _ctx.SaveChangesAsync(); }
    }
}
