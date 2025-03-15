using System.Text;
using Plugins.DevTool;

namespace Plugins.Decoding;

public class DecodingBase64 : IDevToolPlugin
{
    public int id => 2;

    public string Name => "DecodingBase64";

    public string Category => "Decode";

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
