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
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public int Categoryid { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(false)]
        public bool IsPremiumTool { get; set; }

        [Required]
        public string AccessiableRole { get; set; }

        [ForeignKey("AccessiableRole")]
        public IdentityRole Role { get; set; }
    }
}