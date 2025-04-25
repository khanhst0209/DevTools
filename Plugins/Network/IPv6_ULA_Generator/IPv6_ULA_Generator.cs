using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DevTool.Categories;
using DevTool.Input2Execute.IPv6_ULA_Generator;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace IPv6_ULA_Generator;

public class IPv6_ULA_Generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "IPv6 ULA Generator";

    public Category Category => Category.Network;

    public string Description { get; set; } = "Generate your own local, non-routable IP addresses for your network according to RFC4193.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M4 21c1.147-4.02 1.983-8.027 2-12h6c.017 3.973.853 7.98 2 12""></path><path d=""M12.5 13H17c.025 2.612.894 5.296 2 8""></path><path d=""M9 5a2.4 2.4 0 0 1 2-1a2.4 2.4 0 0 1 2 1a2.4 2.4 0 0 0 2 1a2.4 2.4 0 0 0 2-1a2.4 2.4 0 0 1 2-1a2.4 2.4 0 0 1 2 1""></path><path d=""M3 21h19""></path></g></svg>";
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
                        id = "ipv6Ula",
                        label = "IPv6 ULA",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "firstBlock",
                        label = "First routable block",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "lastBlock",
                        label = "Last routable block",
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
        var myInput = JsonSerializer.Deserialize<IPv6UlaGeneratorInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        string macAddress = myInput.macAddress?.Trim();

        if (string.IsNullOrEmpty(macAddress))
        {
            return new { error = "Please enter a valid MAC address" };
        }

        try
        {
            // Normalize MAC address
            string normalizedMac = NormalizeMacAddress(macAddress);

            // Generate the Global ID based on MAC address (40 bits / 5 bytes)
            byte[] globalId = GenerateGlobalId(normalizedMac);

            // Create the ULA prefix - fd + 40 bits (5 bytes) of Global ID
            string ulaPrefix = FormatUlaPrefix(globalId);

            // Calculate first and last routable blocks - now with /64 subnet notation
            string firstBlock = $"{ulaPrefix}:0::/64";  // Subnet ID 0
            string lastBlock = $"{ulaPrefix}:ffff::/64"; // Last Subnet ID ffff

            return new
            {
                ipv6Ula = ulaPrefix + "::/48",
                firstBlock = firstBlock,
                lastBlock = lastBlock
            };
        }
        catch (Exception ex)
        {
            return new { error = $"Error generating IPv6 ULA: {ex.Message}" };
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

    private byte[] GenerateGlobalId(string normalizedMac)
    {
        // Convert MAC to bytes
        byte[] macBytes = new byte[6];
        for (int i = 0; i < 6; i++)
        {
            macBytes[i] = Convert.ToByte(normalizedMac.Substring(i * 2, 2), 16);
        }

        // Add timestamp for extra randomness
        byte[] timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

        // Combine MAC and timestamp for input to hash function
        byte[] combined = macBytes.Concat(timestamp).ToArray();

        // Generate SHA-1 hash (we only need 40 bits = 5 bytes)
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] hash = sha1.ComputeHash(combined);

            // Take first 5 bytes, set the 7th bit to 1 (according to RFC 4193)
            byte[] globalId = new byte[5];
            Array.Copy(hash, globalId, 5);

            return globalId;
        }
    }

    private string FormatUlaPrefix(byte[] globalId)
    {
        // Format as fd + 5-byte global ID
        return $"fd{BitConverter.ToString(globalId).Replace("-", "").ToLower()}";
    }

    public string GetSheme1()
    {
        return JsonSerializer.Serialize(schema);
    }
}