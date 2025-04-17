using System.ComponentModel.DataAnnotations;

namespace DevTools.Application.Dto.user
{
    public class PremiumUpgradeRequestDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool IsAccepted { get; set; } = true;
    }
}