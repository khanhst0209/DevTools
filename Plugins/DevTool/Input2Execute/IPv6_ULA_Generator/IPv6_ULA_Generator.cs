using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DevTool.Input2Execute.IPv6_ULA_Generator
{
    public class IPv6UlaGeneratorInput
    {
        [Required(ErrorMessage = "MAC address is required")]
        [RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$|^([0-9A-Fa-f]{12})$",
            ErrorMessage = "Invalid MAC address format. Use format: XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX")]
        public string macAddress { get; set; }
    }
}