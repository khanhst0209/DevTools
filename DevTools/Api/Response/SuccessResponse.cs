namespace DevTools.Api.Response
{
    public class SuccessResponse<T> 
    {
        public T Data { get; set; }
        public bool Success => true;

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}