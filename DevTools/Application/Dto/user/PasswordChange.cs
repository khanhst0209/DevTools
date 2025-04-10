using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.user
{
    public class PasswordChangeDTO
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}