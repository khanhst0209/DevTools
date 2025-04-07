
namespace DevTools.Exceptions.AccountManager.RegisterException
{
    public class UserCreationFailedException : Exception
    {
        public UserCreationFailedException(string message) : base(message) { }
    }

    public class RoleAssignmentFailedException : Exception
    {
        public RoleAssignmentFailedException(string message) : base(message) { }
    }
}