using System.ComponentModel.DataAnnotations;

namespace DevTools.Domain.Entities
{
    public class PremiumUpgradeRequest
    {
        [Key]
        public string UserId { get; set; }
    }
}