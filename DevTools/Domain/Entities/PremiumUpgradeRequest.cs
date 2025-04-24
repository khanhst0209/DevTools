using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyWebAPI.data;

namespace DevTools.Domain.Entities
{
    public class PremiumUpgradeRequest
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}