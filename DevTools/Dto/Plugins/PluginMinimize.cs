using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.Plugins
{
    public class PluginMinimize
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public string Name {get; set;}
    }
}