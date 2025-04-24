using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.IPv4_Address_Converter
{
    public class IPv4AddressConverterInput
    {
        [Required(ErrorMessage = "IPv4 address is required")]
        [RegularExpression(@"^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$",
            ErrorMessage = "Invalid IPv4 address format. Use format: xxx.xxx.xxx.xxx")]
        public string ipv4Address { get; set; }
    }
}