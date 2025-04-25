using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DevTool.Categories;
using DevTool.Input2Execute.IPv4_Address_Converter;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace IPv4_Address_Converter;

public class IPv4_Address_Converter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "IPv4 Address Converter";

    public Category Category => Category.Network;

    public string Description { get; set; } = "Convert an IP address into decimal, binary, hexadecimal, or even an IPv6 representation of it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M11 10V5h-1m8 14v-5h-1""></path><rect x=""15"" y=""5"" width=""3"" height=""5"" rx="".5""></rect><rect x=""10"" y=""14"" width=""3"" height=""5"" rx="".5""></rect><path d=""M6 10h.01M6 19h.01""></path></g></svg>";
    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "ipv4Address",
                        label = "The IPv4 address",
                        type = ComponentType.text.ToString(),
                        defaultValue = "192.168.1.1",
                        placeholder = "e.g. 192.168.1.1",
                        required = true
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "dec",
                        label = "Decimal",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "hex",
                        label = "Hexadecimal",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "bin",
                        label = "Binary",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "ipv6Full",
                        label = "IPv6",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "ipv6Short",
                        label = "IPv6 (short)",
                        type = ComponentType.text.ToString()
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<IPv4AddressConverterInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        // Parse the IPv4 address
        if (!IPAddress.TryParse(myInput.ipv4Address, out IPAddress ipAddress) ||
            ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return new { error = "Invalid IPv4 address format." };
        }

        try
        {
            byte[] ipBytes = ipAddress.GetAddressBytes();
            uint ipUint = (uint)((ipBytes[0] << 24) | (ipBytes[1] << 16) | (ipBytes[2] << 8) | ipBytes[3]);

            // Decimal representation
            string decimalValue = ipUint.ToString();

            // Hexadecimal representation
            string hexValue = "0x" + ipUint.ToString("X8");

            // Binary representation with dots
            string[] binaryOctets = new string[4];
            for (int i = 0; i < 4; i++)
            {
                binaryOctets[i] = Convert.ToString(ipBytes[i], 2).PadLeft(8, '0');
            }
            string binaryValue = string.Join(".", binaryOctets);

            // IPv6 representation - full format
            byte[] ipv6Bytes = new byte[16];
            // Fill first 10 bytes with 0s
            for (int i = 0; i < 10; i++)
            {
                ipv6Bytes[i] = 0;
            }
            // Set bytes 10-11 to 0xFF (for ::ffff: prefix)
            ipv6Bytes[10] = 0xFF;
            ipv6Bytes[11] = 0xFF;
            // Copy IPv4 bytes to last 4 bytes
            Array.Copy(ipBytes, 0, ipv6Bytes[12..], 0, 4);

            // Create the full IPv6 representation
            string ipv6Full = string.Join(":",
    BitConverter.ToString(ipv6Bytes, 0, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 2, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 4, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 6, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 8, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 10, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 12, 2).Replace("-", ""),
    BitConverter.ToString(ipv6Bytes, 14, 2).Replace("-", "")
).ToLower();


            // Create the short IPv6 representation (::ffff:IPv4)
            string ipv6Short = "::ffff:" + ipAddress.ToString();

            return new
            {
                dec = decimalValue,
                hex = hexValue,
                bin = binaryValue,
                ipv6Full = ipv6Full,
                ipv6Short = ipv6Short
            };
        }
        catch (Exception ex)
        {
            return new { error = $"Error converting IPv4 address: {ex.Message}" };
        }
    }

    public string GetSheme1()
    {
        return JsonSerializer.Serialize(schema);
    }
}