namespace MyWebAPI.Dto
{
    public class ErrorRespones
    {
        public bool Success { get; set; } = false;
        public String Message { get; set; }

        // public List<string> details { get; set; }

        public ErrorRespones(string message)
        {
            Message = message;
        }
    }
}