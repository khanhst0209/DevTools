using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.Plugins
{
    public class PluginsResponeDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Category { get; set; }
        public string? Decription { get; set; }
    }
}