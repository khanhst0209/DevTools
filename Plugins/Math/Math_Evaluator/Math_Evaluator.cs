using System.ComponentModel.DataAnnotations;
using DevTool.Categories;
using DevTool.Input2Execute.Math_Evaluator;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;
using Flee;
using Flee.PublicTypes;
namespace Math_Evaluator;

public class Math_Evaluator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Math evaluator";

    public Category Category => Category.Math;


    public string Description { get; set; } = "A calculator for evaluating mathematical expressions. You can use functions like sqrt, cos, sin, abs, etc.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M16 13l4 4m0-4l-4 4""></path><path d=""M20 5h-7L9 19l-3-6H4""></path></g></svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "inputText",
                        label = "Your Math Expression",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = "sqrt(4)",
                        rows = 1
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textoutput",
                        label = "Your Answer",
                        type = ComponentType.textarea.ToString(),
                        rows = 1
                    }
                }
            },
        }
    };

    private object EvaluateMathExpression(string expr)
    {
        var context = new ExpressionContext();

        context.Imports.AddType(typeof(Math));

        try
        {
            IDynamicExpression e = context.CompileDynamic(expr);
            return e.Evaluate();
        }
        catch (ExpressionCompileException ex)
        {
            Console.WriteLine("Flee Compile Error: " + ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Flee Runtime Error: " + ex.Message);
            return null;
        }
    }

    public object Execute(object input)
    {
        var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = System.Text.Json.JsonSerializer.Serialize(dict);
        var myInput = System.Text.Json.JsonSerializer.Deserialize<Math_EvaluatorInput>(json);
        Console.WriteLine("Mapped Input: " + System.Text.Json.JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        try
        {
            var result = EvaluateMathExpression(myInput.inputText);
            return new { textoutput = result };
        }
        catch (Exception ex)
        {
            return new { textoutput = "" };
        }
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
