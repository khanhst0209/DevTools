using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.TemperatureConverterInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Temperature_Converter;

public class Temperature_Converter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Temperature converter";

    public Category Category => Category.Measurement;


    public string Description { get; set; } = "Degrees temperature conversions for Kelvin, Celsius, Fahrenheit, Rankine, Delisle, Newton, Réaumur, and Rømer.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M10 13.5a4 4 0 1 0 4 0V5a2 2 0 0 0-4 0v8.5""></path><path d=""M10 9h4""></path></g></svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "number",
                        label = "Enter temperature value",
                        type = ComponentType.number.ToString(),
                        required = true
                    },
                    new SchemaInput{
                        id = "fromUnit",
                        label = "From",
                        type = ComponentType.dropdown.ToString(),
                        required = true,
                        options = Enum.GetNames(typeof(TemperatureUnit)).Select(x => new ComponentOption{ value = x, label = x }).ToList()
                    },
                    new SchemaInput{
                        id = "toUnit",
                        label = "To",
                        type = ComponentType.dropdown.ToString(),
                        required = true,
                        options = Enum.GetNames(typeof(TemperatureUnit)).Select(x => new ComponentOption{ value = x, label = x }).ToList()
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "result",
                        label = "Converted temperature",
                        type = ComponentType.text.ToString()
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
            var json = JsonSerializer.Serialize(dict);
            var myInput = JsonSerializer.Deserialize<TemperatureCoverterInput>(json);

            Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

            var fromUnit = Enum.Parse<TemperatureUnit>(myInput.fromUnit);
            var toUnit = Enum.Parse<TemperatureUnit>(myInput.toUnit);
            var value = myInput.value;

            double result = ConvertTemperature(value, fromUnit, toUnit);

            return new Dictionary<string, object>
        {
            { "result", result }
        };
        }
        catch
        {
            return new Dictionary<string, object>
        {
            { "result", "" }
        };
        }
    }

    private double ConvertTemperature(double value, TemperatureUnit from, TemperatureUnit to)
    {
        // Chuyển về Celsius trước
        double celsius = from switch
        {
            TemperatureUnit.Celsius => value,
            TemperatureUnit.Fahrenheit => (value - 32) * 5 / 9,
            TemperatureUnit.Kelvin => value - 273.15,
            TemperatureUnit.Rankine => (value - 491.67) * 5 / 9,
            TemperatureUnit.Delisle => 100 - value * 2 / 3,
            TemperatureUnit.Newton => value * 100 / 33,
            TemperatureUnit.Reaumur => value * 5 / 4,
            TemperatureUnit.Romer => (value - 7.5) * 40 / 21,
            _ => throw new ArgumentException("Invalid 'from' unit")
        };

        // Chuyển từ Celsius sang đơn vị mong muốn
        return to switch
        {
            TemperatureUnit.Celsius => celsius,
            TemperatureUnit.Fahrenheit => celsius * 9 / 5 + 32,
            TemperatureUnit.Kelvin => celsius + 273.15,
            TemperatureUnit.Rankine => (celsius + 273.15) * 9 / 5,
            TemperatureUnit.Delisle => (100 - celsius) * 3 / 2,
            TemperatureUnit.Newton => celsius * 33 / 100,
            TemperatureUnit.Reaumur => celsius * 4 / 5,
            TemperatureUnit.Romer => celsius * 21 / 40 + 7.5,
            _ => throw new ArgumentException("Invalid 'to' unit")
        };
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
