using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.EscapeHTMLEntityInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace EscapeHTMLEntity;

public class EscapeHTMLEntity : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Escape HTML entities";

    public Category Category => Category.Web;


    public string Description { get; set; } = "Encode text to URL-encoded format (also known as\"percent-encoded\"), or decode from it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M7 8l-4 4l4 4""></path>
            <path d=""M17 8l4 4l-4 4""></path>
            <path d=""M14 4l-4 16""></path>
        </g>
    </svg>";




    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
        new UISchema {
            name = "Escape HTML",
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "rawHtml",
                    label = "Your String",
                    type = ComponentType.textarea.ToString(),
                    rows = 2,
                    defaultValue = "<title>Group 22, hello</title>",
                    placeholder = "Enter text to escape"
                },
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "escapedHtml",
                    label = "Your string escaped ",
                    type = ComponentType.textarea.ToString(),
                    rows = 2,
                    placeholder = "Escaped HTML will appear here"
                }
            }
        },
        new UISchema {
            name = "Unescape HTML",
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "escapedInput",
                    label = "Your escaped string",
                    type = ComponentType.textarea.ToString(),
                    rows = 2,
                    defaultValue = "&lt;title&gt;IT Tool&lt;/title&gt;",
                    placeholder = "Enter escaped HTML here"
                },
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "unescapedHtml",
                    label = "Your string unescaped :",
                    type = ComponentType.textarea.ToString(),
                    rows = 2,
                    placeholder = "Unescaped result will appear here"
                }
            }
        }
    }
    };



    private string EscapeHtml(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return System.Net.WebUtility.HtmlEncode(input);
    }

    private string UnescapeHtml(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return System.Net.WebUtility.HtmlDecode(input);
    }

    public object Execute(object input)
    {
        Console.WriteLine("======================================================");
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<EscapeHTMLEntityInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");
        Console.WriteLine("======================================================");

        return new
        {
            escapedHtml = EscapeHtml(myInput.rawHtml),
            unescapedHtml = UnescapeHtml(myInput.escapedInput)
        };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
