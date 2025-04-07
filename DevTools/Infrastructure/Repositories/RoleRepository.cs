using DevTools.Exceptions.AccountManager.RoleException;
using DevTools.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DevTools.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public RoleManager<IdentityRole> _roleManager;
        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<string> GetIdByName(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if(role == null)
                throw new RoleWithNameNotFound(name);
            
            return role.Id;
        }

    }
}