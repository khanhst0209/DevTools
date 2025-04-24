using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.RandomPort;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Random_Port_Generator;

public class Random_Port_Generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Random port generator";

    public Category Category => Category.Development;


    public string Description { get; set; } = "Generate random port numbers outside of the range of \"known\" ports (0-1023).";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""3"" y=""4"" width=""18"" height=""8"" rx=""3""></rect><rect x=""3"" y=""12"" width=""18"" height=""8"" rx=""3""></rect><path d=""M7 8v.01""></path><path d=""M7 16v.01""></path></g></svg>";

    private static Random ran = new Random();


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "minValue",
                        label = "Your min port",
                        type = ComponentType.number.ToString(),
                        defaultValue = 1024
                    },
                    new SchemaInput{
                        id = "maxValue",
                        label = "Your max port",
                        type = ComponentType.number.ToString(),
                        defaultValue = 99999
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "portOutput",
                        label = "Your Port",
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
        var myInput = JsonSerializer.Deserialize<RandomPortInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        // Validate object
        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        if (myInput.minValue > myInput.maxValue)
            return new { portOutput = "" };
        int value = ran.Next(myInput.minValue, myInput.maxValue + 1);

        return new { portOutput = value };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
