using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.user
{
    public class RegisterDTO
    {
        [Required]
        public string? UserName { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}