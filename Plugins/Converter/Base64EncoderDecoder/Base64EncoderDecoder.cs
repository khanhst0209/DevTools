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




  private string ToNumeronym(string word)
  {
    if (word.Length <= 2)
      return word;

    char firstChar = word[0];
    char lastChar = word[word.Length - 1];
    int middleCount = word.Length - 2;

    return $"{firstChar}{middleCount}{lastChar}";
  }

  public object Execute(object input)
  {
    if (input is Dictionary<string, string> dictionary && dictionary.ContainsKey("word"))
    {
      string word = dictionary["word"];
      string numeronym = ToNumeronym(word);
      return new { Hash_text = numeronym };
    }

    return "Invalid input format";
  }

  public string GetSheme1()
  {
    return "";
  }
}