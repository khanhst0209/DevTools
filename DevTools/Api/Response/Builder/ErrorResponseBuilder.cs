using DevTools.Api.Response;

public class ErrorResponseBuilder
{
    private readonly ErrorResponse _response = new();

    public ErrorResponseBuilder WithErrors(object errors)
    {
        _response.Errors = errors;
        return this;
    }

    public ErrorResponseBuilder WithStatusCode(int statusCode)
    {
        _response.StatusCode = statusCode;
        return this;
    }

    public ErrorResponseBuilder WithMessage(string message)
    {
        _response.Message = message;
        return this;
    }

    public ErrorResponse Build()
    {
        return _response;
    }
}
