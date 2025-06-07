using System.Text.Json;
using DevTools.Exceptions;

namespace DevTools.Api.Middleware
{
    public class GlobalHandlingMiddleware
    {
        private readonly RequestDelegate _next;


        public GlobalHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";
                var errorResponse = new ErrorResponseBuilder()
                                    .WithErrors(null)
                                    .WithStatusCode(ex.StatusCode)
                                    .WithMessage(ex.Message)
                                    .Build();
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var errorResponse = new ErrorResponseBuilder()
                                    .WithErrors(null)
                                    .WithStatusCode(500)
                                    .WithMessage(ex.Message)
                                    .Build();
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }

}