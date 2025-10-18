using SprintCsharp.Application.Interfaces;
using SprintCsharp.Domain.Entities;

namespace SprintCsharp.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> GetAllUsers() => _repository.GetAll();

        public User? GetUserById(int id) => _repository.GetById(id);

        public void CreateUser(User user) => _repository.Add(user);

        public void UpdateUser(User user) => _repository.Update(user);

        public void DeleteUser(int id) => _repository.Delete(id);
    }
}
