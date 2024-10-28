using ASP.ExceptionsHandler.Models;

namespace ASP.ExceptionsHandler.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllAsync();

        Task<Todo> GetByIdAsync(int id);

        Task<Todo> CreateAsync(CreateTodoRequest request);

        Task<Todo> UpdateAsync(int id, UpdateTodoRequest request);

        Task DeleteAsync(int id);
    }
}