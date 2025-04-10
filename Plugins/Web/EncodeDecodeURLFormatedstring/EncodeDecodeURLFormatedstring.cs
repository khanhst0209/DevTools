using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.EncodeDecodeURLFormatedstringInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace EncodeDecodeURLFormatedstring;

public class EncodeDecodeURLFormatedstring : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Encode/decode URL-formatted strings";

    public Category Category => Category.Web;


    public string Description { get; set; } = "Encode text to URL-encoded format (also known as \"percent-encoded\"), or decode from it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M10 14a3.5 3.5 0 0 0 5 0l4-4a3.5 3.5 0 0 0-5-5l-.5.5""></path>
            <path d=""M14 10a3.5 3.5 0 0 0-5 0l-4 4a3.5 3.5 0 0 0 5 5l.5-.5""></path>
        </g>
    </svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
        new UISchema {
            name = "Encode to URL format",
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "yourString",
                    label = "Your String",
                    type = ComponentType.text.ToString(),
                    defaultValue = "hello world"
                }
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "yourStringHashed",
                    label = "URL-Encoded Result",
                    type = ComponentType.text.ToString(),
                    placeholder = "Your encoded string will appear here..."
                }
            }
        },
        new UISchema {
            name = "Decode from URL format",
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "yourEncodedString",
                    label = "Your Encoded String",
                    type = ComponentType.text.ToString(),
                    defaultValue = "hello%20world"
                }
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "yourStringDecoded",
                    label = "Decoded Result",
                    type = ComponentType.text.ToString(),
                    placeholder = "Decoded string will appear here..."
                }
            }
        }
    }
    };





    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<EncodeDecodeURLFormatedstringInput>(json);

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);

        string encoded = string.IsNullOrEmpty(myInput.yourString)
            ? ""
            : Uri.EscapeDataString(myInput.yourString);

        string decoded = string.IsNullOrEmpty(myInput.yourEncodedString)
            ? ""
            : Uri.UnescapeDataString(myInput.yourEncodedString);

        return new
        {
            yourStringHashed = encoded,
            yourStringDecoded = decoded
        };
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
