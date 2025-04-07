using System.ComponentModel.DataAnnotations;

namespace DevTools.Dto.Plugins
{
    public class PluginsResponeDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public bool? IsPremium { get; set; }
        public string? Icon { get; set; }
    }
}