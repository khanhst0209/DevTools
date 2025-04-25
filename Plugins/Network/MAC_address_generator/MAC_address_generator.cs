using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DevTool.Categories;
using DevTool.Input2Execute.MAC_Address_Generator;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace MAC_address_generator;

public class MAC_address_generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "MAC address generator";

    public Category Category => Category.Network;

    public string Description { get; set; } = "Enter the quantity and prefix. MAC addresses will be generated in your chosen case (uppercase or lowercase)";

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
                        id = "quantity",
                        label = "Quantity",
                        type = ComponentType.number.ToString(),
                        defaultValue = 3,
                        min = 1
                    },
                    new SchemaInput{
                        id = "prefix",
                        label = "MAC address prefix",
                        type = ComponentType.text.ToString(),
                        defaultValue = "64:16:7F",
                        placeholder = "e.g. 64:16:7F"
                    },
                    new SchemaInput{
                        id = "case",
                        label = "Case",
                        type = ComponentType.radio.ToString(),
                        defaultValue = "uppercase",
                        options = new List<ComponentOption>{
                            new ComponentOption { value = "uppercase", label = "Uppercase" },
                            new ComponentOption { value = "lowercase", label = "Lowercase" }
                        }
                    },
                    new SchemaInput{
                        id = "separator",
                        label = "Separator",
                        type = ComponentType.radio.ToString(),
                        defaultValue = ":",
                        options = new List<ComponentOption>{
                            new ComponentOption { value = ":", label = ":" },
                            new ComponentOption { value = "-", label = "-" },
                            new ComponentOption { value = "none", label = "None" }
                        }
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "macAddresses",
                        label = "Generated MAC addresses",
                        type = ComponentType.textarea.ToString(),
                        rows = 5,
                        placeholder = "Generated addresses will appear here...",
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<MACAddressGeneratorInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        int quantity = myInput.quantity;
        string prefix = myInput.prefix?.Trim() ?? "";
        bool isUppercase = myInput.@case == "uppercase";
        string separator = myInput.separator == "none" ? "" : myInput.separator;

        // Validate and process prefix if provided
        byte[] prefixBytes = new byte[0];
        int remainingOctets = 6; // MAC address has 6 octets total

        if (!string.IsNullOrEmpty(prefix))
        {
            // Remove any separators from the prefix for processing
            string normalizedPrefix = prefix.Replace(":", "").Replace("-", "");

            // Validate prefix format (must be hexadecimal)
            if (!Regex.IsMatch(normalizedPrefix, "^[0-9A-Fa-f]+$"))
            {
                return new { error = "Invalid MAC address prefix. Must be hexadecimal characters." };
            }

            // Ensure prefix length is even and not too long
            if (normalizedPrefix.Length % 2 != 0 || normalizedPrefix.Length > 10)
            {
                return new { error = "Invalid prefix length. Must be an even number of hex digits and less than or equal to 10 digits." };
            }

            // Convert prefix to bytes
            prefixBytes = new byte[normalizedPrefix.Length / 2];
            for (int i = 0; i < normalizedPrefix.Length; i += 2)
            {
                prefixBytes[i / 2] = Convert.ToByte(normalizedPrefix.Substring(i, 2), 16);
            }

            remainingOctets = 6 - prefixBytes.Length;
        }

        // Generate MAC addresses
        var random = new Random();
        var addresses = new List<string>();

        for (int i = 0; i < quantity; i++)
        {
            byte[] macBytes = new byte[6];

            // Copy prefix if provided
            if (prefixBytes.Length > 0)
            {
                Array.Copy(prefixBytes, macBytes, prefixBytes.Length);
            }

            // Generate random bytes for the remaining octets
            for (int j = 6 - remainingOctets; j < 6; j++)
            {
                macBytes[j] = (byte)random.Next(256);
            }

            // Format the MAC address
            string macAddress = FormatMacAddress(macBytes, separator, isUppercase);
            addresses.Add(macAddress);
        }

        return new
        {
            macAddresses = string.Join(Environment.NewLine, addresses)
        };
    }

    private string FormatMacAddress(byte[] macBytes, string separator, bool isUppercase)
    {
        // Create format strings with different placeholder indices
        string[] formatParts = new string[6];
        for (int i = 0; i < 6; i++)
        {
            formatParts[i] = "{" + i + ":X2}";
        }

        var formatString = string.Join(separator, formatParts);

        if (!isUppercase)
        {
            formatString = formatString.ToLower();
        }

        return string.Format(
            formatString,
            macBytes[0], macBytes[1], macBytes[2], macBytes[3], macBytes[4], macBytes[5]
        );
    }

    public string GetSheme1()
    {
        return JsonSerializer.Serialize(schema);
    }
}