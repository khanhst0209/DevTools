using System.Text;
using Plugins.DevTool;


namespace Plugins.Encode
{
    public class EncodingBase64 : IDevToolPlugin
    {
        public int id => 1;
        public string Name => "EncodingBase64";

        public string Category => "";

        public object Execute(object input)
        {
            if(input is not string)
            {
                throw new Exception("Invalid input");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(input.ToString());
            string base64Encoded = Convert.ToBase64String(bytes);
            return base64Encoded;
        }

    }
}