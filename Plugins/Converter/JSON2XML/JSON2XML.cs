using System.Xml;
using DevTool.Categories;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;


using System.Xml;
using Newtonsoft.Json;
using DevTool.Input2Execute.Json2XMLInput;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace JSON2XML;

public class JSON2XML : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "JSON to XML";

    public Category Category => Category.Converter;


    public string Description { get; set; } = "Convert JSON to XML";

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
                        label = "Your JSON content",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = "{\"a\":{\"_attributes\":{\"x\":\"1.234\",\"y\":\"It's\"}}}",
                        rows = 8
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textoutput",
                        label = "Converted XML",
                        type = ComponentType.xml.ToString(),
                        rows = 8
                    }
                }
            },
        }
    };

    public string ConvertJsonToXmlWithAttributes(string json)
    {
        try
        {
            JObject obj = JObject.Parse(json);

            XmlDocument xmlDoc = new XmlDocument();
            foreach (var prop in obj.Properties())
            {
                XmlElement root = xmlDoc.CreateElement(prop.Name);
                AddJsonToXml(prop.Value as JObject, root, xmlDoc);
                xmlDoc.AppendChild(root);
            }

            return xmlDoc.OuterXml;
        }
        catch
        {
            return "";
        }
    }

    private static void AddJsonToXml(JObject json, XmlElement xmlElement, XmlDocument xmlDoc)
    {
        foreach (var prop in json.Properties())
        {
            if (prop.Name == "_attributes" && prop.Value is JObject attrs)
            {
                foreach (var attr in attrs.Properties())
                {
                    xmlElement.SetAttribute(attr.Name, attr.Value.ToString());
                }
            }
            else if (prop.Value is JObject childObj)
            {
                XmlElement childElement = xmlDoc.CreateElement(prop.Name);
                AddJsonToXml(childObj, childElement, xmlDoc);
                xmlElement.AppendChild(childElement);
            }
            else
            {
                XmlElement child = xmlDoc.CreateElement(prop.Name);
                child.InnerText = prop.Value.ToString();
                xmlElement.AppendChild(child);
            }
        }
    }

    public object Execute(object input)
    {
        var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = System.Text.Json.JsonSerializer.Serialize(dict);
        var myInput = System.Text.Json.JsonSerializer.Deserialize<Json2XMLInput>(json);
        Console.WriteLine("Mapped Input: " + System.Text.Json.JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        return new
        {
            textoutput = ConvertJsonToXmlWithAttributes(myInput.inputText)
        };

    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
