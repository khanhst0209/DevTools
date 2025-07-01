using DevTools.Exceptions;

namespace DevTools.Application.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(404, message)
        {
        }
    }
}