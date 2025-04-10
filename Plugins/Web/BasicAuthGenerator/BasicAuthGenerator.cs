using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.BasicAuthInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace BasicAuthGenerator;

public class BasicAuthGenerator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Basic auth generator";

    public Category Category => Category.Web;


    public string Description { get; set; } = "Encode text to URL-encoded format (also known as \"percent-encoded\"), or decode from it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <path d=""M3 17h18c.55 0 1 .45 1 1s-.45 1-1 1H3c-.55 0-1-.45-1-1s.45-1 1-1zm-.5-4.43c.36.21.82.08 1.03-.28l.47-.82l.48.83c.21.36.67.48 1.03.28c.36-.21.48-.66.28-1.02l-.49-.84h.95c.41 0 .75-.34.75-.75s-.34-.75-.75-.75H5.3l.47-.82c.21-.36.09-.82-.27-1.03a.764.764 0 0 0-1.03.28L4 8.47l-.47-.82a.764.764 0 0 0-1.03-.28c-.36.21-.48.67-.27 1.03l.47.82h-.95c-.41 0-.75.34-.75.75s.34.75.75.75h.95l-.48.83c-.2.36-.08.82.28 1.02zm8 0c.36.21.82.08 1.03-.28l.47-.82l.48.83c.21.36.67.48 1.03.28c.36-.21.48-.66.28-1.02l-.48-.83h.95c.41 0 .75-.34.75-.75s-.34-.75-.75-.75h-.96l.47-.82a.76.76 0 0 0-.27-1.03a.746.746 0 0 0-1.02.27l-.48.82l-.47-.82a.742.742 0 0 0-1.02-.27c-.36.21-.48.67-.27 1.03l.47.82h-.96a.74.74 0 0 0-.75.74c0 .41.34.75.75.75h.95l-.48.83c-.2.36-.08.82.28 1.02zM23 9.97c0-.41-.34-.75-.75-.75h-.95l.47-.82a.76.76 0 0 0-.27-1.03a.746.746 0 0 0-1.02.27l-.48.83l-.47-.82a.742.742 0 0 0-1.02-.27c-.36.21-.48.67-.27 1.03l.47.82h-.95a.743.743 0 0 0-.76.74c0 .41.34.75.75.75h.95l-.48.83a.74.74 0 0 0 .28 1.02c.36.21.82.08 1.03-.28l.47-.82l.48.83c.21.36.67.48 1.03.28c.36-.21.48-.66.28-1.02l-.48-.83h.95c.4-.01.74-.35.74-.76z"" fill=""currentColor""></path>
    </svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
        new UISchema {
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "username",
                    label = "Username",
                    type = ComponentType.text.ToString()
                },
                new SchemaInput{
                    id = "password",
                    label = "Password",
                    type = ComponentType.text.ToString()
                }
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "authHeader",
                    label = "Authorization header:",
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
        var myInput = JsonSerializer.Deserialize<BasicAuthInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        var result = GenerateBasicAuthHeader(myInput.username, myInput.password);

        return new { authHeader = result };
    }

    private string GenerateBasicAuthHeader(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return "Authorization: Basic ";

        var plainText = $"{username}:{password}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        var base64 = Convert.ToBase64String(bytes);
        return $"Authorization: Basic {base64}";
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
