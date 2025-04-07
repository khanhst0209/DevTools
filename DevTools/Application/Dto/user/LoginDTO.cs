using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.user
{
    public class LoginDTO
    {
        [Required]
        public string UserName {get; set;}
        [Required]
        public string password {get; set;}
        
    }
}