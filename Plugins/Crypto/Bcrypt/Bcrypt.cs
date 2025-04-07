using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;
using DevTool.UISchema;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using DevTool.Input2Execute.Bcrypt;

namespace Bcrypt;
public class Bcrypt : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Bcrypt";

    public Category Category => Category.Crypto;


    public string Description { get; set; } = "Hash and compare text string using bcrypt. Bcrypt is a password-hashing function based on the Blowfish cipher.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 24 24'>
    <g fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>
        <rect x='8' y='11' width='8' height='5' rx='1'></rect>
        <path d='M10 11V9a2 2 0 1 1 4 0v2'></path>
        <rect x='4' y='4' width='16' height='16' rx='2'></rect>
    </g>
</svg>";



    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                name = "Hash",
                inputs = new List<SchemaInput>{
                  new SchemaInput{
                        id = "text2hash",
                        label = "Your string",
                        type = ComponentType.textarea.ToString(),
                        placeholder = "Your string to bcrypt...",
                        rows = 2
                    },
                    new SchemaInput{
                        id = "saltCount",
                        label = "Salt count",
                        type = ComponentType.number.ToString(),
                        defaultValue = 10,
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "hashResult",
                        label = "Your text hashed:",
                        type = ComponentType.textarea.ToString(),
                        rows = 4,
                        placeholder = "Hashed result will appear here..."
                    }
                }
            },

            new UISchema {
                name = "Compare string with hash",
                inputs = new List<SchemaInput>{
                  new SchemaInput{
                        id = "text2compare",
                        label = "Your string",
                        type = ComponentType.textarea.ToString(),
                        placeholder = "Your string to compare",
                        rows = 2
                    },
                    new SchemaInput{
                        id = "hashtext",
                        label = "Your string",
                        type = ComponentType.textarea.ToString(),
                        placeholder = "Your hash to compare",
                        rows = 2
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "compareResult",
                        label = "Do they match ?",
                        type = ComponentType.text.ToString(),
                        placeholder = "Your result is here"
                    }
                }
            }
        }
    };

    public object Execute(object input)
    {
        Console.WriteLine("======================================================");
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<BcryptInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");
        Console.WriteLine("======================================================");

        var result = new { hashResult = "", compareResult = "No" };

        if (!string.IsNullOrEmpty(myInput.text2hash) && myInput.saltCount > 0)
        {
            result = new
            {
                hashResult = BCrypt.Net.BCrypt.HashPassword(myInput.text2hash, workFactor: myInput.saltCount),
                compareResult = result.compareResult
            };
        }

        if (!string.IsNullOrEmpty(myInput.text2compare) && !string.IsNullOrEmpty(myInput.hashtext))
        {
            try
            {
                bool isMatch = BCrypt.Net.BCrypt.Verify(myInput.text2compare, myInput.hashtext);
                result = new
                {
                    hashResult = result.hashResult,
                    compareResult = isMatch ? "Yes" : "No"
                };
            }
            catch
            {
                result = new
                {
                    hashResult = result.hashResult,
                    compareResult = "No"
                };
            }
        }

        return result;

    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
