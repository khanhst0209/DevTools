using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Xml;
using DevTool.Categories;
using DevTool.Input2Execute.JSONMinifyInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace XML_Formatter;

public class XML_Formatter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "XML formatter";

    public Category Category => Category.Development;


    public string Description { get; set; } = "Prettify your XML string into a friendly, human-readable format.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M7 8l-4 4l4 4""></path><path d=""M17 8l4 4l-4 4""></path><path d=""M14 4l-4 16""></path></g></svg>";



    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "textInput",
                        label = "Your raw XML",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = @"<hello><world>foo</world><world>bar</world></hello>"
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textOutput",
                        label = "Formatted XML from your XML",
                        type = ComponentType.xml.ToString()
                    }
                }
            },
        }
    };


    public object Execute(object input)
    {
        // Deserialize đầu vào thành Dictionary
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<JsonMinifyInput>(json); // dùng chung input

        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        // Validate object
        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        // Prettify XML
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(myInput.textInput); // Thử load XML từ chuỗi

            var stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = false,
                OmitXmlDeclaration = false
            };

            using (var writer = XmlWriter.Create(stringBuilder, settings))
            {
                xmlDoc.Save(writer); 
            }

            var formattedXml = stringBuilder.ToString();

            return new { textOutput = formattedXml };
        }
        catch (Exception)
        {
            return new { textOutput = "" };
        }
    }



    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
