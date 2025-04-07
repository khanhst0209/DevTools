using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyWebAPI.data
{
    public class User : IdentityUser
    {
        public string FullName {get; set;} = "Unknown";
        public bool IsPremium {get; set;} = false;
    }
}