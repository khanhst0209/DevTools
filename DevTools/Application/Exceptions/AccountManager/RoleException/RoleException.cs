namespace DevTools.Exceptions.AccountManager.RoleException
{
    public class RoleWithNameNotFound : Exception
    {
        public RoleWithNameNotFound(string name) : base($"Role with name : {name} is Not Found"){}
    }
}