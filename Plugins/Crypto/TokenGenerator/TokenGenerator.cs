using DevTool.Categories;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;
using DevTool.Input2Execute.TokenGenerator;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace TokenGenerator;

public class TokenGenerator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Token Generator";

    public Category Category => Category.Crypto;


    public string Description { get; set; } = "Generate random string with the chars you want, uppercase or lowercase letters, numbers and/or symbols.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 24 24'>
    <g fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>
        <path d='M18 4l3 3l-3 3'></path>
        <path d='M18 20l3-3l-3-3'></path>
        <path d='M3 7h3a5 5 0 0 1 5 5a5 5 0 0 0 5 5h5'></path>
        <path d='M21 7h-5a4.978 4.978 0 0 0-2.998.998M9 16.001A4.984 4.984 0 0 1 6 17H3'></path>
    </g>
</svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "toggle",
                        type = ComponentType.toggle.ToString(),
                        options = new List<ComponentOption>{
                            new ComponentOption{
                                value = "nut1",
                                label = "Uppercase (ABC...)"
                            },
                            new ComponentOption{
                                value = "nut2",
                                label = "Numbers (123...)"
                            },
                            new ComponentOption{
                                value = "nut3",
                                label = "Lowercase (abc...)"
                            },
                            new ComponentOption{
                                value = "nut4",
                                label = "Symbols (!-;...)"
                            }
                        },
                    },
                    new SchemaInput{
                        id = "slider",
                        label = "Length",
                        type = ComponentType.slider.ToString(),
                        min = 1,
                        max = 512,
                        step = 1
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textoutput",
                        label = "Result token:",
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
        var myInput = JsonSerializer.Deserialize<TokenGeneratorInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        var result = GenerateToken(myInput);
        return new { textoutput = result };
    }


    private string GenerateToken(TokenGeneratorInput input)
    {
        var charPool = "";

        if (input.toggle.TryGetValue("nut1", out bool useUpper) && useUpper)
            charPool += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Console.WriteLine("==========================3");
        if (input.toggle.TryGetValue("nut3", out bool useLower) && useLower)
            charPool += "abcdefghijklmnopqrstuvwxyz";

        if (input.toggle.TryGetValue("nut2", out bool useNumber) && useNumber)
            charPool += "0123456789";
        Console.WriteLine("==========================4");

        if (input.toggle.TryGetValue("nut4", out bool useSymbol) && useSymbol)
            charPool += "!@#$%^&*()-_=+[]{}|;:,.<>?";

        if (string.IsNullOrEmpty(charPool))
            return "";

        var random = new Random();
        var chars = new char[input.slider];

        for (int i = 0; i < input.slider; i++)
        {
            chars[i] = charPool[random.Next(charPool.Length)];
        }

        return new string(chars);
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }


}
