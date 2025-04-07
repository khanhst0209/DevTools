namespace DevTools.Repositories.Interfaces
{
    public interface IRoleRepository
    {
         Task<string> GetIdByName(string name);
    }
}