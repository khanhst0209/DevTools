using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.Plugins
{
    public class PluginMinimize
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Icon { get; set; }
        public bool IsPremium { get; set; }
    }
}