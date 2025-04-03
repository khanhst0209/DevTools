using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Base64EncoderDecoder;

public class Base64EncoderDecoder : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Base64 string encoder/decoder";

    public Category Category => Category.Converter;


    public string Description { get; set; } = "Simply encode and decode strings into their base64 representation.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M14 3v4a1 1 0 0 0 1 1h4""></path>
            <rect x=""9"" y=""12"" width=""3"" height=""5"" rx=""1""></rect>
            <path d=""M17 21H7a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h7l5 5v11a2 2 0 0 1-2 2z""></path>
            <path d=""M15 12v5""></path>
        </g>
    </svg>";





    public object Execute(object input)
    {
        throw new NotImplementedException();
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
}
