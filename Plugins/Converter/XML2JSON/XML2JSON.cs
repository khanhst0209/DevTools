using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.XML2Token;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

using System.Xml;
using Newtonsoft.Json;

namespace XML2JSON;

public class XML2JSON : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "XML to JSON";

    public Category Category => Category.Converter;


    public string Description { get; set; } = "Convert XML to JSON";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M7 4a2 2 0 0 0-2 2v3a2 3 0 0 1-2 3a2 3 0 0 1 2 3v3a2 2 0 0 0 2 2""></path>
            <path d=""M17 4a2 2 0 0 1 2 2v3a2 3 0 0 0 2 3a2 3 0 0 0-2 3v3a2 2 0 0 1-2 2""></path>
        </g>
    </svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "inputText",
                        label = "Your XML content",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = "<a x=\"1.234\" y=\"It's\"/>",
                        rows = 8
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textoutput",
                        label = "Converted JSON",
                        type = ComponentType.json.ToString(),
                        rows = 8
                    }
                }
            },
        }
    };

    private string ConvertXmlToJson(string xml)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public object Execute(object input)
    {
        var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = System.Text.Json.JsonSerializer.Serialize(dict);
        var myInput = System.Text.Json.JsonSerializer.Deserialize<XML2JsonInput>(json);
        Console.WriteLine("Mapped Input: " + System.Text.Json.JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        return new
        {
            textoutput = ConvertXmlToJson(myInput.inputText)
        };

    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
