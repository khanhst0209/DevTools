using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.UserPlugin
{
    public class CreateUserPluginDTO
    {
        [Required]
        public int PluginId { get; set; }
    }
}