namespace DevTools.Exceptions.UserPlugin
{
    public class UserPLuginExistedException : Exception
    {
        public UserPLuginExistedException(string userId, int pluginId) : base($"Plugin with Id : {pluginId} is existed in user favorited plugins: {userId}"){}
    }

    public class UserPLuginNotFoundException : Exception
    {
        public UserPLuginNotFoundException(string userId, int pluginId) : base($"Plugin with Id : {pluginId} is not found in user favorited plugins: {userId}"){}
    }
}