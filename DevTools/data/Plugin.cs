using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevTools.data
{
    [Table("Plugin")]
    public class Plugin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue("")]
        public string Icon {get; set;}

        [DefaultValue(false)]
        public bool IsPremium { get; set; }

        [Required]
        public string AccessiableRoleId  { get; set; }
        

        [ForeignKey("AccessiableRoleId")]
        public IdentityRole Role { get; set; }
        
        [ForeignKey("CategoryId")]
        public PluginCategory category {get; set;}
    }
}