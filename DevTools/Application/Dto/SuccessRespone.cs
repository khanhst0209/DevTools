namespace DevTools.Application.Dto
{
    public class SuccessRespone
    {
        public bool Success { get; set; } = true;
        public String Message { get; set; }

        // public List<string> details { get; set; }

        public SuccessRespone(string message)
        {
            Message = message;
        }
    }
}