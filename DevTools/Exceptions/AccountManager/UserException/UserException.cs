namespace DevTools.Exceptions.AccountManager.UserException
{
    public class UserNotFound : Exception
    {
        public UserNotFound(string UserId) : base($"User with Id : {UserId} Is not found"){}
    }
}