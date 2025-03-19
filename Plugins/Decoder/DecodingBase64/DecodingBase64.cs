using System.IO.Pipes;
using System.Text;
using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Plugins.Decoding;

public class DecodingBase64 : IDevToolPlugin
{
    public int id { get; set; }

    public string Name => "DecodingBase64";

    public Category Category => Category.Decode;
    public string Description { get; set; } = "This is Decoding Method used to decode a string(Base 64) to string. Input : string , output : string";
    
    public bool IsActive { get; set; } = true;

    public Roles AccessiableRole { get ; set; } = Roles.Anonymous;
    public bool IsPremiumTool { get; set; } = false;

    public object Execute(object input)
    {
        if (input is not string base64String)
        {
            throw new ArgumentException("Invalid input. Expected a Base64 string.");
        }

        try
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid Base64 format.");
        }
    }
}
