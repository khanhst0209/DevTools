namespace HashText;

using DevTool.Categories;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;
using DevTool.Input2Execute.HashText;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

public class HashText : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Hash Text";

    public Category Category => Category.Crypto;


    public string Description { get; set; } = "Hash a text string using the function you need : MD5, SHA1, SHA256, SHA224, SHA512, SHA384, SHA3 or RIPEMD160";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 24 24'>
    <g fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>
        <path d='M3 3l18 18'></path>
        <path d='M10.584 10.587a2 2 0 0 0 2.828 2.83'></path>
        <path d='M9.363 5.365A9.466 9.466 0 0 1 12 5c4 0 7.333 2.333 10 7c-.778 1.361-1.612 2.524-2.503 3.488m-2.14 1.861C15.726 18.449 13.942 19 12 19c-4 0-7.333-2.333-10-7c1.369-2.395 2.913-4.175 4.632-5.341'></path>
    </g>
</svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "textInput",
                        label = "Your text to hash",
                        type = ComponentType.textarea.ToString(),
                    },
                    new SchemaInput{
                        id = "digitalEncoding",
                        label = "Digital Encoding",
                        placeholder = "Select Digital Encoding",
                        type = ComponentType.dropdown.ToString(),
                        defaultValue = HashTextDigitalEncoding.Binary.ToString(),
                        options = new List<ComponentOption>{
                            new ComponentOption{
                                value = HashTextDigitalEncoding.Binary.ToString(),
                                label = "Binary (base 2)"
                            },
                            new ComponentOption{
                                value = HashTextDigitalEncoding.Hexadecimal.ToString(),
                                label = "Hexadeciaml (base 16)"
                            },
                            new ComponentOption{
                                value = HashTextDigitalEncoding.Base64.ToString(),
                                label = "Base64 (base 64)"
                            },
                            new ComponentOption{
                                value = HashTextDigitalEncoding.Base64url.ToString(),
                                label = "Base64 (base 64 with url safe chars)"
                            }
                        },
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "MD5",
                        label = "MD5",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "SHA1",
                        label = "SHA1",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "SHA256",
                        label = "SHA256",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "SHA512",
                        label = "SHA512",
                        type = ComponentType.text.ToString()
                    },
                    new SchemaOutput{
                        id = "SHA384",
                        label = "SHA384",
                        type = ComponentType.text.ToString()
                    }
                }
            },
        }
    };


    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<HashTextInput>(json);
        Console.WriteLine("============================================");
        Console.WriteLine(input);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        // Khởi tạo dictionary mặc định rỗng
        var hashResults = new Dictionary<string, string>
    {
        { "MD5", "" },
        { "SHA1", "" },
        { "SHA256", "" },
        { "SHA512", "" },
        { "SHA384", "" },
    };
        try
        {
            Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
            Console.WriteLine("Validated Input");

            var encoding = myInput.digitalEncoding;

            if (!Enum.TryParse<HashTextDigitalEncoding>(encoding, out var selectedEncoding))
                throw new Exception("Invalid encoding selection.");

            var text = myInput.textInput;

            // Hash từng thuật toán, gán kết quả vào dictionary
            hashResults["MD5"] = ComputeHash(text, MD5.Create(), selectedEncoding);
            hashResults["SHA1"] = ComputeHash(text, SHA1.Create(), selectedEncoding);
            hashResults["SHA256"] = ComputeHash(text, SHA256.Create(), selectedEncoding);
            hashResults["SHA512"] = ComputeHash(text, SHA512.Create(), selectedEncoding);
            hashResults["SHA384"] = ComputeHash(text, SHA384.Create(), selectedEncoding);


        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during hashing: " + ex.Message);
        }
        Console.WriteLine("============================================");
        return hashResults;
    }


    private string ComputeHash(string input, HashAlgorithm algorithm, HashTextDigitalEncoding encoding)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = algorithm.ComputeHash(bytes);

        return encoding switch
        {
            HashTextDigitalEncoding.Hexadecimal => BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant(),
            HashTextDigitalEncoding.Base64 => Convert.ToBase64String(hashBytes),
            HashTextDigitalEncoding.Base64url => Convert.ToBase64String(hashBytes)
                                                    .Replace("+", "-")
                                                    .Replace("/", "_")
                                                    .Replace("=", ""),
            HashTextDigitalEncoding.Binary => string.Join("", hashBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))),
            _ => throw new Exception("Unsupported encoding")
        };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
