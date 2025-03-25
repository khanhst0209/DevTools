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
        public string icon { get; set; } = @"
        <svg
            className=""w-10 h-10""
            xmlns=""http://www.w3.org/2000/svg""
            viewBox=""0 0 24 24""
            fill=""none""
            stroke=""currentColor""
            stroke-width=""2""
            stroke-linecap=""round""
            stroke-linejoin=""round""
        >
            <path d=""M4 12h16""></path>
            <path d=""M12 4v16""></path>
        </svg>";


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