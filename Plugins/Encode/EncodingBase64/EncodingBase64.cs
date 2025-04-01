using System.Text;
using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;


namespace Plugins.Encode
{
    public class EncodingBase64 : IDevToolPlugin
    {
        public int Id { get; set; }
        public string Name => "EncodingBase64";

        public Category Category => Category.Encode;


        public string Description { get; set; } = "This is Encoding Method used to encode a string into Base 64. Input : string , output : string";

        public Roles AccessiableRole { get; set; } = Roles.Anonymous;
        public bool IsActive { get; set; } = true;
        public bool IsPremium { get; set; } = false;
        public string Icon { get; set; } = @"
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
            if (input is not string)
            {
                throw new Exception("Invalid input");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(input.ToString());
            string base64Encoded = Convert.ToBase64String(bytes);
            return base64Encoded;
        }

        public string GetSheme1()
        {
            return @"{
  ""id"": ""1"",
  ""name"": ""Base64 Encoder"",
  ""description"": ""Encode text to Base64 format"",
  ""category"": ""Encoding"",
  ""icon"": ""<svg>...</svg>"",
  ""isPremium"": false,
  ""uiSchema"": {
    ""inputs"": [
      {
        ""id"": ""text"",
        ""label"": ""Text to encode"",
        ""type"": ""textarea"",
        ""defaultValue"": ""alo"",
        ""placeholder"": ""Enter text to encode..."",
        ""required"": true
      }
    ],
    ""outputs"": [
      {
        ""id"": ""encodedText"",
        ""label"": ""Encoded result"",
        ""type"": ""textarea"",
        ""readonly"": true,
        ""rows"": 8
      }
    ],
    ""controls"": [
      {
        ""id"": ""encode"",
        ""label"": ""Encode"",
        ""type"": ""button"",
        ""primary"": true,
        ""action"": ""encode""
      },
      {
        ""id"": ""clear"",
        ""label"": ""Clear"",
        ""type"": ""button"",
        ""action"": ""clear""
      }
    ],
    ""options"": [
      {
        ""id"": ""urlSafe"",
        ""label"": ""URL-safe encoding"",
        ""type"": ""checkbox"",
        ""defaultValue"": false
      }
    ]
  },
  ""api"": {
    ""process"": ""/api/plugins/1/process""
  }
}";
        }


        public object GetSheme2()
        {
            return @"{
  ""id"": ""1"",
  ""name"": ""Base64 Encoder"",
  ""description"": ""Encode text to Base64 format"",
  ""category"": ""Encoding"",
  ""icon"": ""<svg>...</svg>"",
  ""isPremium"": false,
  ""uiSchema"": {
    ""inputs"": [
      {
        ""id"": ""text"",
        ""label"": ""Text to encode"",
        ""type"": ""textarea"",
        ""defaultValue"": ""alo"",
        ""placeholder"": ""Enter text to encode..."",
        ""required"": true
      }
    ],
    ""outputs"": [
      {
        ""id"": ""encodedText"",
        ""label"": ""Encoded result"",
        ""type"": ""textarea"",
        ""readonly"": true,
        ""rows"": 8
      }
    ],
    ""controls"": [
      {
        ""id"": ""encode"",
        ""label"": ""Encode"",
        ""type"": ""button"",
        ""primary"": true,
        ""action"": ""encode""
      },
      {
        ""id"": ""clear"",
        ""label"": ""Clear"",
        ""type"": ""button"",
        ""action"": ""clear""
      }
    ],
    ""options"": [
      {
        ""id"": ""urlSafe"",
        ""label"": ""URL-safe encoding"",
        ""type"": ""checkbox"",
        ""defaultValue"": false
      }
    ]
  },
  ""api"": {
    ""process"": ""/api/plugins/1/process""
  }
}";
        }

    }
}