using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Dto
{
    public class CreateItemDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Length must be lower than 100")]
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId {get; set;}
    }
}