using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;
using DevTool.Input2Execute.IPv4_Subnet_Calculator;

namespace IPv4_Subnet_Calculator;

public class IPv4_Subnet_Calculator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "IPv4 Subnet Calculator";

    public Category Category => Category.Network;


    public string Description { get; set; } = "Parse your IPv4 CIDR blocks and get all the info you need about your subnet.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><path d=""M16 4.2c1.5 0 3 .6 4.2 1.7l.8-.8C19.6 3.7 17.8 3 16 3s-3.6.7-5 2.1l.8.8C13 4.8 14.5 4.2 16 4.2zm-3.3 2.5l.8.8c.7-.7 1.6-1 2.5-1s1.8.3 2.5 1l.8-.8c-.9-.9-2.1-1.4-3.3-1.4s-2.4.5-3.3 1.4zM19 13h-2V9h-2v4H5c-1.1 0-2 .9-2 2v4c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2v-4c0-1.1-.9-2-2-2zm0 6H5v-4h14v4zM6 16h2v2H6zm3.5 0h2v2h-2zm3.5 0h2v2h-2z"" fill=""currentColor""></path></svg>";
    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "ipAddress",
                        label = "An IPv4 address with or without mask",
                        type = ComponentType.text.ToString(),
                        defaultValue = "192.168.0.0/24",
                        placeholder = "e.g. 192.168.0.0/24",
                        required = true
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "netmask",
                        label = "Netmask",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "networkAddress",
                        label = "Network address",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "networkMask",
                        label = "Network mask",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "networkMaskBinary",
                        label = "Network mask in binary",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "cidrNotation",
                        label = "CIDR notation",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "wildcardMask",
                        label = "Wildcard mask",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "networkSize",
                        label = "Network size",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "firstAddress",
                        label = "First address",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "lastAddress",
                        label = "Last address",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "broadcastAddress",
                        label = "Broadcast address",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "ipClass",
                        label = "IP class",
                        type = ComponentType.text.ToString()
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(input.ToString());
        var ipAddressInput = dict["ipAddress"];

        if (string.IsNullOrWhiteSpace(ipAddressInput))
        {
            return new
            {
                error = "Please provide a valid IPv4 address"
            };
        }

        try
        {
            // Parse CIDR notation
            string ipPart = ipAddressInput;
            int cidrPrefix = 32; // Default to /32 if no prefix is provided

            if (ipAddressInput.Contains("/"))
            {
                string[] parts = ipAddressInput.Split('/');
                ipPart = parts[0];

                if (!int.TryParse(parts[1], out cidrPrefix) || cidrPrefix < 0 || cidrPrefix > 32)
                {
                    return new { error = "Invalid CIDR notation. Prefix must be between 0 and 32." };
                }
            }

            // Parse the IP address
            if (!IPAddress.TryParse(ipPart, out IPAddress ipAddress))
            {
                return new { error = "Invalid IPv4 address format." };
            }

            // Ensure it's an IPv4 address
            if (ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return new { error = "The provided address is not an IPv4 address." };
            }

            byte[] ipBytes = ipAddress.GetAddressBytes();
            uint ipUint = (uint)(ipBytes[0] << 24 | ipBytes[1] << 16 | ipBytes[2] << 8 | ipBytes[3]);

            // Calculate network mask from CIDR
            uint maskUint = cidrPrefix == 0 ? 0 : ~((1u << (32 - cidrPrefix)) - 1);
            byte[] maskBytes = new byte[]
            {
                (byte)(maskUint >> 24),
                (byte)(maskUint >> 16),
                (byte)(maskUint >> 8),
                (byte)maskUint
            };

            // Calculate network address
            uint networkUint = ipUint & maskUint;
            byte[] networkBytes = new byte[]
            {
                (byte)(networkUint >> 24),
                (byte)(networkUint >> 16),
                (byte)(networkUint >> 8),
                (byte)networkUint
            };

            // Calculate wildcard mask
            uint wildcardUint = ~maskUint;
            byte[] wildcardBytes = new byte[]
            {
                (byte)(wildcardUint >> 24),
                (byte)(wildcardUint >> 16),
                (byte)(wildcardUint >> 8),
                (byte)wildcardUint
            };

            // Calculate network size (number of hosts)
            uint networkSize = wildcardUint + 1;
            long usableHosts = networkSize - 2;
            if (usableHosts < 0) usableHosts = 0;

            // Calculate first host address
            uint firstHostUint = networkSize <= 2 ? networkUint : networkUint + 1;
            byte[] firstHostBytes = new byte[]
            {
                (byte)(firstHostUint >> 24),
                (byte)(firstHostUint >> 16),
                (byte)(firstHostUint >> 8),
                (byte)firstHostUint
            };

            // Calculate last host address
            uint broadcastUint = networkUint | wildcardUint;
            uint lastHostUint = networkSize <= 2 ? broadcastUint : broadcastUint - 1;
            byte[] lastHostBytes = new byte[]
            {
                (byte)(lastHostUint >> 24),
                (byte)(lastHostUint >> 16),
                (byte)(lastHostUint >> 8),
                (byte)lastHostUint
            };

            // Calculate broadcast address
            byte[] broadcastBytes = new byte[]
            {
                (byte)(broadcastUint >> 24),
                (byte)(broadcastUint >> 16),
                (byte)(broadcastUint >> 8),
                (byte)broadcastUint
            };

            // Determine IP class
            string ipClass = DetermineIpClass(ipBytes[0]);

            // Create network mask in binary
            string binaryMask = string.Join(".", maskBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            // Create formatted outputs
            IPAddress netmaskIP = new IPAddress(maskBytes);
            IPAddress networkIP = new IPAddress(networkBytes);
            IPAddress firstHostIP = new IPAddress(firstHostBytes);
            IPAddress lastHostIP = new IPAddress(lastHostBytes);
            IPAddress broadcastIP = new IPAddress(broadcastBytes);
            IPAddress wildcardIP = new IPAddress(wildcardBytes);

            return new
            {
                netmask = netmaskIP.ToString(),
                networkAddress = networkIP.ToString(),
                networkMask = netmaskIP.ToString(),
                networkMaskBinary = binaryMask,
                cidrNotation = $"{networkIP}/{cidrPrefix}",
                wildcardMask = wildcardIP.ToString(),
                networkSize = $"{networkSize} ({(networkSize > 2 ? usableHosts : 0)} usable)",
                firstAddress = firstHostIP.ToString(),
                lastAddress = lastHostIP.ToString(),
                broadcastAddress = broadcastIP.ToString(),
                ipClass = ipClass
            };
        }
        catch (Exception ex)
        {
            return new { error = $"Error processing IP address: {ex.Message}" };
        }
    }

    private string DetermineIpClass(byte firstOctet)
    {
        if (firstOctet < 128)
            return "Class A";
        else if (firstOctet < 192)
            return "Class B";
        else if (firstOctet < 224)
            return "Class C";
        else if (firstOctet < 240)
            return "Class D (Multicast)";
        else
            return "Class E (Reserved/Experimental)";
    }

    public string GetSheme1()
    {
        return JsonSerializer.Serialize(schema);
    }
}