namespace DevTools.Application.Exceptions.AccountManager.ChangePassword
{
    public class UnvalidConfirmPasswordException : Exception
    {
        public UnvalidConfirmPasswordException(string password, string confirmPassword) : base($"New Password : {password} is not equal to Confirm Password : {confirmPassword} ") { }
    }

    public class PasswordChangeFailedException : Exception
    {
        public PasswordChangeFailedException(IEnumerable<string> errors)
            : base(string.Join("; ", errors)) { }
    }

}