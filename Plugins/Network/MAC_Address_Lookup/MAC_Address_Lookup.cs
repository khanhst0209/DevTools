using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevTool.Categories;
using DevTool.Input2Execute.MAC_Address_Lookup;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace MAC_Address_Lookup;

public class MAC_Address_Lookup : IDevToolPlugin
{
    private readonly HttpClient _httpClient;

    public MAC_Address_Lookup()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "DevTools MAC Lookup");
    }

    public int Id { get; set; }
    public string Name => "MAC Address Lookup";

    public Category Category => Category.Network;

    public string Description { get; set; } = "Find the vendor and manufacturer of a device by its MAC address.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""13"" y=""8"" width=""8"" height=""12"" rx=""1""></rect><path d=""M18 8V5a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h9""></path><path d=""M16 9h2""></path></g></svg>";
    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "macAddress",
                        label = "MAC address",
                        type = ComponentType.text.ToString(),
                        placeholder = "e.g. 20:37:06:12:34:56",
                        defaultValue = "20:37:06:12:34:56",
                        required = true
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "vendorInfo",
                        label = "Vendor info",
                        type = ComponentType.textarea.ToString(),
                        rows = 4,
                        resize = Enum.Parse<ComponentResize>("y"),
                        placeholder = "Manufacturer info will appear here..."
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<MACAddressLookupInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        string macAddress = myInput.macAddress?.Trim();

        if (string.IsNullOrEmpty(macAddress))
        {
            return new { vendorInfo = "Please enter a valid MAC address" };
        }

        try
        {
            // Normalize MAC address format
            macAddress = NormalizeMacAddress(macAddress);

            // Get the OUI (first 3 octets)
            string oui = GetOUI(macAddress);

            // Lookup vendor info
            string vendorInfo = LookupVendorAsync(oui).GetAwaiter().GetResult();

            return new { vendorInfo = vendorInfo };
        }
        catch (Exception ex)
        {
            return new { vendorInfo = $"Error looking up MAC address: {ex.Message}" };
        }
    }

    private string NormalizeMacAddress(string macAddress)
    {
        // Remove any separators and convert to uppercase
        string normalized = macAddress.Replace(":", "").Replace("-", "").Replace(".", "").ToUpper();

        // Validate MAC address format
        if (!Regex.IsMatch(normalized, "^[0-9A-F]{12}$"))
        {
            throw new ArgumentException("Invalid MAC address format. Expected format: XX:XX:XX:XX:XX:XX");
        }

        return normalized;
    }

    private string GetOUI(string normalizedMac)
    {
        // Extract first 6 characters (first 3 bytes/octets)
        return normalizedMac.Substring(0, 6);
    }

    private async Task<string> LookupVendorAsync(string oui)
    {
        try
        {
            string url = $"https://api.macaddress.io/v1?apiKey=at_fE9bFiEKp7aAxbGXpif9zjdCcEhvC&output=json&search={oui}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<DevTool.Input2Execute.MAC_Address_Lookup.MacLookupResponse>(jsonResult);
                return result?.VendorDetails?.CompanyName + "\n" +
                       result?.VendorDetails?.CompanyAddress + "\n" +
                       result?.VendorDetails?.CountryCode;
            }
            return $"No vendor information found for MAC address with OUI: {FormatOuiWithColons(oui)}";
        }
        catch (Exception ex)
        {
            return $"Error during vendor lookup: {ex.Message}";
        }
    }

    private string FormatOuiWithColons(string oui)
    {
        return string.Format("{0}:{1}:{2}",
            oui.Substring(0, 2),
            oui.Substring(2, 2),
            oui.Substring(4, 2));
    }

    public string GetSheme1()
    {
        return JsonSerializer.Serialize(schema);
    }
}