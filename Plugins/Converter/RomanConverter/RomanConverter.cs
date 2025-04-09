using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.RomanConverterInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace RomanConverter;

public class RomanConverter : IDevToolPlugin
{
  public int Id { get; set; }
  public string Name => "Roman numeral converter";

  public Category Category => Category.Converter;


  public string Description { get; set; } = "Convert Roman numerals to numbers and convert numbers to Roman numerals.";

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

  public Schema schema => new Schema
  {
    id = Id,
    uiSchemas = new List<UISchema>{
            new UISchema {
                name = "Arabic to roman",
                inputs = new List<SchemaInput>{
                  new SchemaInput{
                        id = "numberToHash",
                        label = "Your Number",
                        type = ComponentType.number.ToString(),
                        defaultValue = 2004,
                        min = 1,
                        max = 3999
                    },
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "romanResult",
                        label = "Your Roman",
                        type = ComponentType.textarea.ToString(),
                        rows = 1,
                        placeholder = "Roman result will appear here..."
                    }
                }
            },

            new UISchema {
                name = "Roman to arabic",
                inputs = new List<SchemaInput>{
                  new SchemaInput{
                        id = "romanText",
                        label = "Your Roman",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = "MMIV",
                        rows = 1
                    },

                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "numberResult",
                        label = "Your Number",
                        type = ComponentType.text.ToString(),
                        placeholder = "Your number will appear here",
                        rows = 1
                    },
                }
            }
        }
  };
  private readonly Dictionary<char, int> RomanToIntMap = new()
    {
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D', 500},
        {'M', 1000}
    };

  private readonly (int Value, string Symbol)[] IntToRomanMap = new[]
  {
        (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
        (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
        (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
    };

  private string ConvertToRoman(int number)
  {
    if (number <= 0 || number > 3999) return "";

    var sb = new StringBuilder();
    foreach (var (val, symbol) in IntToRomanMap)
    {
      while (number >= val)
      {
        sb.Append(symbol);
        number -= val;
      }
    }
    return sb.ToString();
  }

  private int ConvertFromRoman(string roman)
  {
    if (string.IsNullOrEmpty(roman)) return -1;

    int total = 0;
    int prev = 0;

    foreach (char c in roman.ToUpperInvariant())
    {
      if (!RomanToIntMap.TryGetValue(c, out int current)) return -1;

      total += current > prev ? current - 2 * prev : current;
      prev = current;
    }

    return total;
  }

  public object Execute(object input)
  {
    Console.WriteLine("======================================================");
    var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
    var json = JsonSerializer.Serialize(dict);
    var myInput = JsonSerializer.Deserialize<RomanConverterInput>(json);
    Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

    Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
    Console.WriteLine("Validated Input");
    Console.WriteLine("======================================================");

    return new
    {
      romanResult = ConvertToRoman(myInput.numberToHash),
      numberResult = ConvertFromRoman(myInput.romanText).ToString()
    };

  }

  public string GetSheme1()
  {
    return "";
  }
}