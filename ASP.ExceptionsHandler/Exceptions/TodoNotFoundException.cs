namespace ASP.ExceptionsHandler.Exceptions
{
    public class TodoNotFoundException : Exception
    {
        public TodoNotFoundException(int id)
            : base($"Todo with ID {id} was not found")
        {
        }
    }
}