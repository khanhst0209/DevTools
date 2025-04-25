using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;
using DevTool.Input2Execute.Time_Converter;

namespace Time_Converter;
public class Time_Converter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Time Converter";

    public Category Category => Category.Measurement;

    public string Description { get; set; } = "Convert time between different units (seconds, minutes, hours, days).";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""12"" cy=""12"" r=""10""></circle><polyline points=""12 6 12 12 16 14""></polyline></g></svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "inputTime",
                        label = "Enter time value",
                        type = ComponentType.number.ToString(),
                        defaultValue = 1,
                        min = 0
                    },
                    new SchemaInput{
                        id = "fromUnit",
                        label = "Convert from",
                        type = ComponentType.dropdown.ToString(),
                        defaultValue = "seconds",
                        options = new List<ComponentOption>{
                            new ComponentOption { value = "seconds", label = "Seconds" },
                            new ComponentOption { value = "minutes", label = "Minutes" },
                            new ComponentOption { value = "hours", label = "Hours" },
                            new ComponentOption { value = "days", label = "Days" }
                        }
                    },
                    new SchemaInput{
                        id = "toUnit",
                        label = "Convert to",
                        type = ComponentType.dropdown.ToString(),
                        defaultValue = "minutes",
                        options = new List<ComponentOption>{
                            new ComponentOption { value = "seconds", label = "Seconds" },
                            new ComponentOption { value = "minutes", label = "Minutes" },
                            new ComponentOption { value = "hours", label = "Hours" },
                            new ComponentOption { value = "days", label = "Days" }
                        }
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "convertedResult",
                        label = "Converted result",
                        type = ComponentType.text.ToString(),
                        placeholder = "Result will appear here..."
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<TimeConverterInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        double result = ConvertTime(myInput.inputTime, myInput.fromUnit, myInput.toUnit);

        // Format the result with appropriate precision
        string formattedResult;
        formattedResult = result.ToString();

        // Add unit to the result
        formattedResult += " " + GetUnitLabel(myInput.toUnit);

        return new { convertedResult = formattedResult };
    }

    private double ConvertTime(double value, string fromUnit, string toUnit)
    {
        // First convert to seconds as the base unit
        double valueInSeconds = fromUnit switch
        {
            "seconds" => value,
            "minutes" => value * 60,
            "hours" => value * 3600,
            "days" => value * 86400,
            _ => value // default to seconds
        };

        // Then convert from seconds to the target unit
        return toUnit switch
        {
            "seconds" => valueInSeconds,
            "minutes" => valueInSeconds / 60,
            "hours" => valueInSeconds / 3600,
            "days" => valueInSeconds / 86400,
            _ => valueInSeconds // default to seconds
        };
    }

    private string GetUnitLabel(string unit)
    {
        return unit switch
        {
            "seconds" => "second(s)",
            "minutes" => "minute(s)",
            "hours" => "hour(s)",
            "days" => "day(s)",
            _ => unit
        };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}