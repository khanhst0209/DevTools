using System.ComponentModel.DataAnnotations;
using DevTool.Categories;
using DevTool.Input2Execute.Percentage_Calculator;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Percentage_Calculator;

public class Percentage_Calculator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Percentage calculator";

    public Category Category => Category.Math;


    public string Description { get; set; } = "Easily calculate percentages from a value to another value, or from a percentage to a value.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""17"" cy=""17"" r=""1""></circle><circle cx=""7"" cy=""7"" r=""1""></circle><path d=""M6 18L18 6""></path></g></svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                name = "Percentages from a value to another value",
                inputs = new List<SchemaInput>{
                  new SchemaInput{
                        id = "percentageInput",
                        label = "Your percentage:",
                        type = ComponentType.number.ToString(),
                        defaultValue = 50
                    },
                    new SchemaInput{
                        id = "totalInput_1",
                        label = "Your Number",
                        type = ComponentType.number.ToString(),
                        defaultValue = 100
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "numberOutput",
                        label = "Your result",
                        type = ComponentType.textarea.ToString(),
                        placeholder = "Your result will appear here..."
                    }
                }
            },

            new UISchema {
                name = "From a percentage to a value",
                inputs = new List<SchemaInput>{
                  new SchemaInput{
                        id = "partInput",
                        label = "Your Number to find percentage",
                        type = ComponentType.number.ToString(),
                        defaultValue = 50
                    },
                    new SchemaInput{
                        id = "totalInput_2",
                        label = "Your Number",
                        type = ComponentType.number.ToString(),
                        defaultValue = 100
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "percentageOutput",
                        label = "Your result",
                        type = ComponentType.textarea.ToString(),
                        placeholder = "Your result will appear here..."
                    }
                }
            }
        }
    };



    private float CalculatePercentageOfValue(float percent, float total)
    {
        return (percent / 100) * total;
    }

    // Case 2: Calculate what percent X is of Y
    private double CalculateWhatPercent(float part, float total)
    {
        if (total == 0) throw new DivideByZeroException("Total cannot be zero.");
        return (part / total) * 100;
    }


    public object Execute(object input)
    {
        var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = System.Text.Json.JsonSerializer.Serialize(dict);
        var myInput = System.Text.Json.JsonSerializer.Deserialize<Percentage_CalculatorInput>(json);
        Console.WriteLine("Mapped Input: " + System.Text.Json.JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        string case_1 = "";
        string case_2 = "";

        // Case 1: Tính X% của Y
        if (myInput.percentageInput != null && myInput.totalInput_1 != null)
        {
            case_1 = CalculatePercentageOfValue(myInput.percentageInput, myInput.totalInput_1).ToString();
        }

        // Case 2: Tính X là bao nhiêu phần trăm của Y
        if (myInput.partInput != null && myInput.totalInput_2 != null && myInput.totalInput_2 != 0)
        {
            case_2 = CalculateWhatPercent(myInput.partInput, myInput.totalInput_2).ToString();
        }

        return new
        {
            numberOutput = case_1,
            percentageOutput = case_2
        };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
