namespace DevTools.Exceptions.AccountManager.UserException
{
    public class UserNotFound : BaseNotFoundException
    {
        public UserNotFound(string UserId) : base($"User with Id : {UserId} Is not found"){}
    }
}