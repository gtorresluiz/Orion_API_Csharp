using Microsoft.AspNetCore.Mvc;
using SprintCsharp.Domain.Entities;
using SprintCsharp.Application.Services;

namespace SprintCsharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_service.GetAllUsers());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            _service.CreateUser(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User updatedUser)
        {
            // Busca o usuário existente
            var existingUser = _service.GetUserById(id);
            if (existingUser == null)
                return NotFound(); // Retorna 404 se não existir

            // Atualiza os campos do usuário existente
            existingUser.Id = existingUser.Id;
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Balance = updatedUser.Balance;
            existingUser.PreferredInvestment = updatedUser.PreferredInvestment;
            existingUser.Level = updatedUser.Level;
            existingUser.UpdatedAt = updatedUser.UpdatedAt;

            
            // Adicione aqui outros campos que devem ser atualizados

            // Salva as alterações
            _service.UpdateUser(existingUser);

            return NoContent(); // 204 No Content para indicar sucesso
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteUser(id);
            return NoContent();
        }
    }
}
