using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.String_Obfuscator;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace String_Obfuscator;

public class String_Obfuscator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "String obfuscator";

    public Category Category => Category.Text;


    public string Description { get; set; } = "Obfuscate a string (like a secret, an IBAN, or a token) to make it shareable and identifiable without revealing its content.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M3 3l18 18""></path><path d=""M10.584 10.587a2 2 0 0 0 2.828 2.83""></path><path d=""M9.363 5.365A9.466 9.466 0 0 1 12 5c4 0 7.333 2.333 10 7c-.778 1.361-1.612 2.524-2.503 3.488m-2.14 1.861C15.726 18.449 13.942 19 12 19c-4 0-7.333-2.333-10-7c1.369-2.395 2.913-4.175 4.632-5.341""></path></g></svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema> {
        new UISchema {
            inputs = new List<SchemaInput> {
                new SchemaInput {
                    id = "textToObfuscate",
                    label = "String to obfuscate",
                    type = ComponentType.text.ToString(),
                },
                new SchemaInput {
                    id = "keepFirst",
                    label = "Keep first",
                    type = ComponentType.number.ToString(),
                    min = 0,
                    max = 100,
                    step = 1
                },
                new SchemaInput {
                    id = "keepLast",
                    label = "Keep last",
                    type = ComponentType.number.ToString(),
                    min = 0,
                    max = 100,
                    step = 1
                },
                new SchemaInput {
                    id = "keepSpaces",
                    label = "Keep spaces",
                    type = ComponentType.toggle.ToString(),
                    options = new List<ComponentOption> {
                        new ComponentOption { value = "IsKeepSpace", label = "Keep Space?" }
                    }
                }
            },
            outputs = new List<SchemaOutput> {
                new SchemaOutput {
                    id = "obfuscatedText",
                    label = "Obfuscated String:",
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
        var myInput = JsonSerializer.Deserialize<StringObfuscatorInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        var result = ObfuscateString(myInput);
        return new { obfuscatedText = result };
    }

    private string ObfuscateString(StringObfuscatorInput input)
    {
        var text = input.textToObfuscate ?? "";
        var keepFirst = input.keepFirst;
        var keepLast = input.keepLast;
        var keepSpaces = input.keepSpaces.TryGetValue("IsKeepSpace", out bool val1) && val1;

        if (text.Length == 0)
            return "";

        if (keepFirst + keepLast > text.Length)
            return "Invalid length range";

        var firstPart = text.Substring(0, keepFirst);
        var lastPart = text.Substring(text.Length - keepLast);
        var middleRaw = text.Substring(keepFirst, text.Length - keepFirst - keepLast);

        var middlePart = new string(middleRaw.Select(c =>
            (keepSpaces && c == ' ') ? ' ' : '*'
        ).ToArray());

        return firstPart + middlePart + lastPart;
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
