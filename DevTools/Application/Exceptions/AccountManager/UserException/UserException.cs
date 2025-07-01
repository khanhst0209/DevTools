using DevTools.Application.Exceptions;

namespace DevTools.Exceptions.AccountManager.UserException
{
    public class UserNotFound : NotFoundException
    {
        public UserNotFound(string UserId) : base($"User with Id : {UserId} is not found"){}
    }
}