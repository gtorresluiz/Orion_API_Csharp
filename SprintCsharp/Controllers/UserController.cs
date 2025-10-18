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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteUser(id);
            return NoContent();
        }
    }
}
