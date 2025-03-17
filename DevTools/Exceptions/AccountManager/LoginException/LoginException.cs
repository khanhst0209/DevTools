namespace DevTools.Exceptions.AccountManager.LoginException
{
    public class InvalidUsernameOrPassword : Exception
    {
        public InvalidUsernameOrPassword(string username, string password) : base($"UserName : {username} or Password : {password} is invalid, please check again!!") { }
    }
}