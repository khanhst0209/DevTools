using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.user
{
    public class LoginDTO
    {
        [Required]
        public string username {get; set;}
        [Required]
        public string password {get; set;}
        
    }
}