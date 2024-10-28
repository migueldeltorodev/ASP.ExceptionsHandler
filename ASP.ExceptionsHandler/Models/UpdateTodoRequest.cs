namespace ASP.ExceptionsHandler.Models
{
    public class UpdateTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}