using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.IPv4_Subnet_Calculator
{
    public class IPv4_Subnet_Calculator
    {
        [Required(ErrorMessage = "IP Address is required")]
        [RegularExpression(@"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})(?:/(\d{1,2}))?$",
            ErrorMessage = "Invalid IPv4 address format. Use format: xxx.xxx.xxx.xxx or xxx.xxx.xxx.xxx/xx")]
        public string ipAddress { get; set; }
    }
}