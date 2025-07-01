namespace DevTools.Api.Response
{
    public class ErrorResponse
    {
        public Object? Errors { get; set; }

        public bool Success => false;

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}