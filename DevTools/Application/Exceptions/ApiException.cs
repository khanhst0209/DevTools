namespace DevTools.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public ApiException(int statusCode, string massage) : base(massage)
        {
            StatusCode = statusCode;
        }
    }
}