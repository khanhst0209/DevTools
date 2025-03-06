using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Dto
{
    public class ItemDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId {get; set;}
    }
}