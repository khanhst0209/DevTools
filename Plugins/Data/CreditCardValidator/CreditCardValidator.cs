using System.Text.Json;
using System.Text.RegularExpressions;
using DevTool.Categories;
using DevTool.Input2Execute.CreditCardValidatorInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace CreditCardValidator;

public class CreditCardValidator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Credit Card Validator";
    public Category Category => Category.Data;

    public string Description { get; set; } = "Validate credit card numbers using the Luhn algorithm, and parse card type, BIN, last 4 digits, and a formatted version.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"" mr-3="""" h-30px="""" w-30px="""" shrink-0="""" op-50="""" fill=""currentColor"">
  <g transform=""scale(0.025) translate(0, 960)"">
    <path d=""M880-720v480q0 33-23.5 56.5T800-160H160q-33 0-56.5-23.5T80-240v-480q0-33 23.5-56.5T160-800h640q33 0 56.5 23.5T880-720Zm-720 80h640v-80H160v80Zm0 160v240h640v-240H160Zm0 240v-480 480Z"" />
  </g>
</svg>";


    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>
        {
            new UISchema
            {
                name = "Credit Card Validator",
                inputs = new List<SchemaInput>
                {
                    new SchemaInput
                    {
                        id = "cardNumber",
                        label = "Credit Card Number",
                        type = ComponentType.text.ToString(),
                        defaultValue = "4111 1111 1111 1111"
                    },
                },
                outputs = new List<SchemaOutput>
                {
                    new SchemaOutput { id = "is_valid", label = "Is Valid?", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "card_type", label = "Card Type", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "bin", label = "BIN (First 6 Digits)", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "last4", label = "Last 4 Digits", type = ComponentType.text.ToString() },
                    new SchemaOutput { id = "formatted", label = "Formatted Number", type = ComponentType.text.ToString() },
                }
            }
        }
    };

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<CreditCardValidatorInput>(json);
        Console.WriteLine("============================================");
        Console.WriteLine(input);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        string rawNumber = myInput.cardNumber?.ToString()?.Replace(" ", "") ?? "";

        var result = new Dictionary<string, string>();

        bool isValid = LuhnCheck(rawNumber);
        result["is_valid"] = isValid ? "Yes" : "No";

        if (rawNumber.Length >= 6)
        {
            result["bin"] = rawNumber.Substring(0, 6);
        }
        else
        {
            result["bin"] = "";
        }

        if (rawNumber.Length >= 4)
        {
            result["last4"] = rawNumber.Substring(rawNumber.Length - 4);
        }
        else
        {
            result["last4"] = "";
        }

        result["card_type"] = GetCardType(rawNumber);
        result["formatted"] = FormatCardNumber(rawNumber);

        return result;
    }

    private bool LuhnCheck(string number)
    {
        int sum = 0;
        bool alternate = false;
        for (int i = number.Length - 1; i >= 0; i--)
        {
            char[] nx = number.ToCharArray();
            int n;
            if (!int.TryParse(nx[i].ToString(), out n)) return false;
            if (alternate)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }
            sum += n;
            alternate = !alternate;
        }
        return (sum % 10 == 0);
    }

    private string FormatCardNumber(string number)
    {
        return string.Join(" ", Regex.Matches(number, ".{1,4}").Select(m => m.Value));
    }

    private string GetCardType(string number)
    {
        if (Regex.IsMatch(number, "^4[0-9]{12}(?:[0-9]{3})?$")) return "Visa";
        if (Regex.IsMatch(number, "^5[1-5][0-9]{14}$")) return "MasterCard";
        if (Regex.IsMatch(number, "^3[47][0-9]{13}$")) return "American Express";
        if (Regex.IsMatch(number, "^6(?:011|5[0-9]{2})[0-9]{12}$")) return "Discover";
        return "Unknown";
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
