using SprintCsharp.Domain.Entities;
using SprintCsharp.Application.Interfaces;
using SprintCsharp.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SprintCsharp.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IEnumerable<User> GetAll() => _context.Users.AsNoTracking().ToList();

        public User? GetById(int id) => _context.Users.Find(id);

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return;

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        private void ConfigureDbContextOptions(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
