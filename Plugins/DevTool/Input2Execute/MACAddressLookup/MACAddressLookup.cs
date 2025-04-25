using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace DevTool.Input2Execute.MAC_Address_Lookup
{
    public class MACAddressLookupInput
    {
        [Required(ErrorMessage = "MAC address is required")]
        [RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$|^([0-9A-Fa-f]{12})$",
            ErrorMessage = "Invalid MAC address format. Use format: XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX")]
        public string macAddress { get; set; }
    }

    public class MacLookupResponse
    {
        [JsonPropertyName("vendorDetails")]
        public VendorDetails VendorDetails { get; set; }
    }

    public class VendorDetails
    {
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("companyAddress")]
        public string CompanyAddress { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }
}