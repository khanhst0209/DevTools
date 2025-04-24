using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.DistanceCoverterInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Distance_Converter;

public class Distance_Converter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Distance converter";

    public Category Category => Category.Measurement;


    public string Description { get; set; } = "Convert between various distance units including meters, kilometers, miles, feet, and more.";

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
                    id = "value",
                    label = "Enter distance value",
                    type = ComponentType.number.ToString(),
                    required = true
                },
                new SchemaInput{
                    id = "fromUnit",
                    label = "From",
                    type = ComponentType.dropdown.ToString(),
                    required = true,
                    defaultValue = "Meter",
                    options = Enum.GetNames(typeof(DistanceUnit))
                        .Select(x => new ComponentOption { value = x.ToLower(), label = x })
                        .ToList()
                },
                new SchemaInput{
                    id = "toUnit",
                    label = "To",
                    type = ComponentType.dropdown.ToString(),
                    required = true,
                    defaultValue = "Kilometer",
                    options = Enum.GetNames(typeof(DistanceUnit))
                        .Select(x => new ComponentOption { value = x.ToLower(), label = x })
                        .ToList()
                }
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "convertedValue",
                    label = "Converted distance",
                    type = ComponentType.text.ToString()
                }
            }
        }
    }
    };


    private static List<ComponentOption> DistanceUnits() => new()
    {
        new ComponentOption { value = "millimeter", label = "Millimeter (mm)" },
        new ComponentOption { value = "centimeter", label = "Centimeter (cm)" },
        new ComponentOption { value = "meter", label = "Meter (m)" },
        new ComponentOption { value = "kilometer", label = "Kilometer (km)" },
        new ComponentOption { value = "inch", label = "Inch (in)" },
        new ComponentOption { value = "foot", label = "Foot (ft)" },
        new ComponentOption { value = "yard", label = "Yard (yd)" },
        new ComponentOption { value = "mile", label = "Mile (mi)" },
        new ComponentOption { value = "nauticalmile", label = "Nautical Mile (nmi)" }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<DistanceConverterInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        double value = myInput.value;
        string from = myInput.fromUnit.ToLower();
        string to = myInput.toUnit.ToLower();

        var conversionRatesToMeter = new Dictionary<string, double>
        {
            ["millimeter"] = 0.001,
            ["centimeter"] = 0.01,
            ["meter"] = 1,
            ["kilometer"] = 1000,
            ["inch"] = 0.0254,
            ["foot"] = 0.3048,
            ["yard"] = 0.9144,
            ["mile"] = 1609.34,
            ["nauticalmile"] = 1852
        };

        if (!conversionRatesToMeter.ContainsKey(from) || !conversionRatesToMeter.ContainsKey(to))
            return new { convertedValue = "" };

        double valueInMeters = value * conversionRatesToMeter[from];
        double converted = valueInMeters / conversionRatesToMeter[to];

        return new { convertedValue = converted.ToString("0.#####") };
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
