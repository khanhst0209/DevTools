using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Dto
{
    public class UpdateItemDTO
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int CategoryId {get; set;}
    }
}