using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyWebAPI.data;

namespace DevTools.data
{
    [Table("UserPlugins")]
    public class UserPlugins
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PluginId { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }

        [ForeignKey("PluginId")]
        public Plugin plugin { get; set; }

    }
}