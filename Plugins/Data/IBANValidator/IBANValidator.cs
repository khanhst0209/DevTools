using System.Text.Json;
using System.Text.RegularExpressions;
using DevTool.Categories;
using DevTool.Input2Execute.IBANValidatorInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace IBANValidator;

public class IBANValidator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "IBAN validator and parser";

    public Category Category => Category.Data;


    public string Description { get; set; } = "Validate and parse IBAN numbers. Check if an IBAN is valid and get the country, BBAN, if it is a QR-IBAN and the IBAN friendly format.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
<svg viewBox=""0 0 24 24"" width=""1.2em"" height=""1.2em"" mr-3="""" h-30px="""" w-30px="""" shrink-0="""" op-50=""""><path fill=""currentColor"" d=""M11.5 1L2 6v2h19V6m-5 4v7h3v-7M2 22h19v-3H2m8-9v7h3v-7m-9 0v7h3v-7H4Z""></path></svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>
        {
            new UISchema
            {
                name = "IBAN Validator",
                inputs = new List<SchemaInput>
                {
                    new SchemaInput
                    {
                        id = "ibanNumber",
                        label = "Your IBAN Number",
                        type = ComponentType.text.ToString(),
                        defaultValue = "FR76 3000 6000 0112 3456 7890 189"
                    },
                },
                outputs = new List<SchemaOutput>
                {
                    new SchemaOutput { id = "is_valid", label = "Is IBAN valid?", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "is_qr_iban", label = "Is IBAN a QR-IBAN?", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "country_code", label = "Country code", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "bban", label = "BBAN", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "friendly_format", label = "IBAN friendly format", type = ComponentType.text.ToString() }
                }
            }
        }
    };


    public object Execute(object input)
    {

        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<IBANValidatorInput>(json);
        Console.WriteLine("============================================");
        Console.WriteLine(input);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));


        var rawIban = myInput.ibanNumber.Replace(" ", "").ToUpper() ?? "";

        var result = new Dictionary<string, string>();

        // Basic check: IBAN must be alphanumeric and between 15–34 chars
        bool isValid = Regex.IsMatch(rawIban, @"^[A-Z0-9]+$") && rawIban.Length >= 15 && rawIban.Length <= 34;

        result["is_valid"] = isValid ? "Yes" : "No";

        if (isValid)
        {
            result["country_code"] = rawIban.Substring(0, 2);
            result["bban"] = rawIban.Substring(4);
            result["friendly_format"] = FormatIbanFriendly(rawIban);
            result["is_qr_iban"] = IsQrIban(rawIban) ? "Yes" : "No";
        }
        else
        {
            result["country_code"] = "";
            result["bban"] = "";
            result["friendly_format"] = "";
            result["is_qr_iban"] = "";
        }

        return result;
    }

    private string FormatIbanFriendly(string iban)
    {
        return string.Join(" ", Enumerable.Range(0, iban.Length / 4 + 1)
            .Select(i => iban.Skip(i * 4).Take(4))
            .Where(g => g.Any())
            .Select(g => new string(g.ToArray())));
    }

    private bool IsQrIban(string iban)
    {
        return iban.StartsWith("CH") && iban.Length >= 12 && iban.Substring(4, 5).StartsWith("3");
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
