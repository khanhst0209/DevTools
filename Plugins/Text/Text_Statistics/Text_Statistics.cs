using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.Text_Statistics;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Text_Statistics;

public class Text_Statistics : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Text statistics";

    public Category Category => Category.Text;


    public string Description { get; set; } = "Get information about a text, the number of characters, the number of words, its size in bytes, ...";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M14 3v4a1 1 0 0 0 1 1h4""></path><path d=""M17 21H7a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h7l5 5v11a2 2 0 0 1-2 2z""></path><path d=""M9 9h1""></path><path d=""M9 13h6""></path><path d=""M9 17h6""></path></g></svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
        new UISchema {
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "textInput",
                    label = "Text",
                    defaultValue = "Developer Tools That Simplify Your Workflow",
                    type = ComponentType.text.ToString()
                }
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "charCount",
                    label = "Character count",
                    type = ComponentType.text.ToString()
                },
                new SchemaOutput{
                    id = "wordCount",
                    label = "Word count",
                    type = ComponentType.text.ToString()
                },
                new SchemaOutput{
                    id = "lineCount",
                    label = "Line count",
                    type = ComponentType.text.ToString()
                },
                new SchemaOutput{
                    id = "byteSize",
                    label = "Byte size",
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
        var myInput = JsonSerializer.Deserialize<TextStatisticsInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        var result = AnalyzeText(myInput.textInput);

        return new
        {
            charCount = result.CharacterCount.ToString(),
            wordCount = result.WordCount.ToString(),
            lineCount = result.LineCount.ToString(),
            byteSize = result.ByteSize.ToString()
        };
    }

    private (int CharacterCount, int WordCount, int LineCount, int ByteSize) AnalyzeText(string text)
    {
        var charCount = text.Length;
        var wordCount = string.IsNullOrWhiteSpace(text) ? 0 :
            text.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var lineCount = text.Split('\n').Length;
        var byteSize = System.Text.Encoding.UTF8.GetByteCount(text);

        return (charCount, wordCount, lineCount, byteSize);
    }



    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
