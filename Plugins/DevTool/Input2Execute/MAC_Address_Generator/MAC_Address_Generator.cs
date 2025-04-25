using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.MAC_Address_Generator
{
    public class MACAddressGeneratorInput
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int quantity { get; set; }

        public string prefix { get; set; }

        [Required]
        public string @case { get; set; }

        [Required]
        public string separator { get; set; }
    }
}