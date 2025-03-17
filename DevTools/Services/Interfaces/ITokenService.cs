
using MyWebAPI.data;

namespace MyWebAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(User user, IList<string> roles);  
    }
}