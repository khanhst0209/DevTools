namespace DevTools.Api.Response
{
    public class SuccessResponseBuilder<T>
    {
        private readonly SuccessResponse<T> _response = new();

        public SuccessResponseBuilder<T> WithData(T data)
        {
            _response.Data = data;
            return this;
        }

        public SuccessResponseBuilder<T> WithStatusCode(int statusCode)
        {
            _response.StatusCode = statusCode;
            return this;
        }

        public SuccessResponseBuilder<T> WithMessage(string message)
        {
            _response.Message = message;
            return this;
        }

        public SuccessResponse<T> Build()
        {
            return _response;
        }
    }
}
