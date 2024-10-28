using ASP.ExceptionsHandler.Models;
using ASP.ExceptionsHandler.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.ExceptionsHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(ITodoService _todoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAll()
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);
            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Create(CreateTodoRequest request)
        {
            var todo = await _todoService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Update(int id, UpdateTodoRequest request)
        {
            var todo = await _todoService.UpdateAsync(id, request);
            return Ok(todo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _todoService.DeleteAsync(id);
            return NoContent();
        }
    }
}