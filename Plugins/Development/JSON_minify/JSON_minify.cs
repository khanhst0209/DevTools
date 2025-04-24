using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.JSONMinifyInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace JSON_minify;

public class JSON_minify : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "JSON minify";

    public Category Category => Category.Development;


    public string Description { get; set; } = "Minify and compress your JSON by removing unnecessary whitespace.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><path d=""M19 5v14H5V5h14m0-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-4.86 8.86l-3 3.87L9 13.14L6 17h12l-3.86-5.14z"" fill=""currentColor""></path></svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "textInput",
                        label = "Your raw JSON",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = @"{""hello"": [""world""]}"
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textOutput",
                        label = "Minified version of your JSON",
                        type = ComponentType.textarea.ToString()
                    }
                }
            },
        }
    };



    public object Execute(object input)
    {
        try
        {
            // Deserialize vào Dictionary trước (nếu bạn cần xử lý thêm)
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
            var json = JsonSerializer.Serialize(dict);
            var myInput = JsonSerializer.Deserialize<JsonMinifyInput>(json);
            Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

            // Validate object
            Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
            Console.WriteLine("Validated Input");

            
            string minified;
            try
            {
                using var doc = JsonDocument.Parse(myInput.textInput);
                minified = JsonSerializer.Serialize(doc.RootElement);
            }
            catch
            {
                minified = "";
            }

            return new { textOutput = minified };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return new { textOutput = "" };
        }
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
