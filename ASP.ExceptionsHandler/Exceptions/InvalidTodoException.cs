namespace ASP.ExceptionsHandler.Exceptions
{
    public class InvalidTodoException : Exception
    {
        public InvalidTodoException(string message)
            : base(message)
        {
        }
    }
}