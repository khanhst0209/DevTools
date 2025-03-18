using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTools.data
{
    [Table("PluginCategory")]
    public class PluginCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public  ICollection<Plugin> Plugins { get; set; } = new List<Plugin>();
    }
}