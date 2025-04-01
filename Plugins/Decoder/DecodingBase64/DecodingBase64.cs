using System.IO.Pipes;
using System.Text;
using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Plugins.Decoding;

public class DecodingBase64 : IDevToolPlugin
{
    public int Id { get; set; }

    public string Name => "DecodingBase64";

    public Category Category => Category.Decode;
    public string Description { get; set; } = "This is Decoding Method used to decode a string(Base 64) to string. Input : string , output : string";

    public bool IsActive { get; set; } = true;

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
                    <svg
                        className=""w-10 h-10""
                        xmlns=""http://www.w3.org/2000/svg""
                        viewBox=""0 0 24 24""
                        fill=""none""
                        stroke=""currentColor""
                        strokeWidth=""2""
                        strokeLinecap=""round""
                        strokeLinejoin=""round""
                    >
                        <rect x=""3"" y=""11"" width=""18"" height=""11"" rx=""2"" ry=""2""></rect>
                        <path d=""M7 11V7a5 5 0 0 1 10 0v4""></path>
                    </svg>";


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

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }

    public object GetSheme2()
    {
        throw new NotImplementedException();
    }
}
