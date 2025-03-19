using System.Text;
using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;


namespace Plugins.Encode
{
    public class EncodingBase64 : IDevToolPlugin
    {
        public string Name => "EncodingBase64";

        public Category Category => Category.Encode;
        
        public int id { get ; set; }

        public string Description { get; set; } = "This is Encoding Method used to encode a string into Base 64. Input : string , output : string";
        
        public Roles AccessiableRole { get; set;} = Roles.Anonymous;
        public bool IsActive { get; set; } = true;
        public bool IsPremiumTool { get; set;} = false;

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