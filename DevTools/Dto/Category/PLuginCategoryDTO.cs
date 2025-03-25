using System.ComponentModel.DataAnnotations;
using DevTools.Dto.Plugins;

namespace DevTools.Dto.Category
{
    public class PLuginCategoryDTO
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public string Name {get; set;}

        public List<PluginMinimize> plugins {get; set;}

    }
}