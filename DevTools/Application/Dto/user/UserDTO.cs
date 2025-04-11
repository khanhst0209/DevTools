using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace DevTools.Dto.user
{
    public class UserDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Role { get; set; }

    }
}