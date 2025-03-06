using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyWebAPI.data
{
    public class User : IdentityUser
    {
     
        public string Name {get; set;} = "Unknown";
        public bool IsPremium {get; set;} = false;
    }
}