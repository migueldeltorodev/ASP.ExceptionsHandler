using ASP.ExceptionsHandler.Exceptions;
using ASP.ExceptionsHandler.Models;

namespace ASP.ExceptionsHandler.Services
{
    public class TodoService : ITodoService
    {
        private static readonly List<Todo> _todos = new();
        private static int _nextId = 1;

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await Task.FromResult(_todos);
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            var todo = await Task.FromResult(_todos.FirstOrDefault(t => t.Id == id));
            if (todo == null)
            {
                throw new TodoNotFoundException(id);
            }
            return todo;
        }

        public async Task<Todo> CreateAsync(CreateTodoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new InvalidTodoException("Title is required");
            }

            var todo = new Todo
            {
                Id = _nextId++,
                Title = request.Title,
                Description = request.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _todos.Add(todo);
            return await Task.FromResult(todo);
        }

        public async Task<Todo> UpdateAsync(int id, UpdateTodoRequest request)
        {
            var todo = await GetByIdAsync(id);

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new InvalidTodoException("Title is required");
            }

            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.IsCompleted = request.IsCompleted;

            return todo;
        }

        public async Task DeleteAsync(int id)
        {
            var todo = await GetByIdAsync(id);
            _todos.Remove(todo);
        }
    }
}