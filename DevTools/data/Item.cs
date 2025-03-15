using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebAPI.data
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        [MaxLength(100)]
        public string Name {get; set;}
        
        [Range(0, double.MaxValue)]
        [DefaultValue(0)]
        public double? Price {get; set;}

    }
    
}