using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.PhoneParser;
using DevTool.Roles;
using DevTool.UISchema;
using PhoneNumbers;
using Plugins.DevTool;

namespace PhoneParser;

public class PhoneParser : IDevToolPlugin
{
    private readonly PhoneNumberUtil _phoneNumberUtil = PhoneNumberUtil.GetInstance();
    public int Id { get; set; }
    public string Name => "Phone parser and formatter";

    public Category Category => Category.Data;


    public string Description { get; set; } = "Parse, validate and format phone numbers. Get information about the phone number, like the country code, type, etc.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"" mr-3="""" h-30px="""" w-30px="""" shrink-0="""" op-50=""""><path d=""M5 4h4l2 5l-2.5 1.5a11 11 0 0 0 5 5L15 13l5 2v4a2 2 0 0 1-2 2A16 16 0 0 1 3 6a2 2 0 0 1 2-2"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""></path></svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>
        {
            new UISchema
            {
                name = "Phone Parser",
                inputs = new List<SchemaInput>
                {
                    new SchemaInput
                    {
                        id = "country",
                        label = "Default country code:",
                        type = ComponentType.dropdown.ToString(), // Dropdown
                        options = GetCountryOptions(),
                        defaultValue = "VN"
                    },
                    new SchemaInput
                    {
                        id = "phone_number",
                        label = "Phone Number",
                        type = ComponentType.text.ToString(),
                        defaultValue = "02838324467"
                    }
                },
                outputs = new List<SchemaOutput>
{
    new SchemaOutput { id = "country_code", label = "Country Code", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "country_name", label = "Country Name", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "is_valid", label = "Is Valid?", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "is_possible", label = "Is Possible?", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "type", label = "Type", type = ComponentType.text.ToString() }, // Loại số (Mobile, FixedLine,...)
    new SchemaOutput { id = "international_format", label = "International Format", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "national_format", label = "National Format", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "e164_format", label = "E.164 Format", type = ComponentType.text.ToString() },
    new SchemaOutput { id = "rfc3966_format", label = "RFC3966 Format", type = ComponentType.text.ToString() }
}

            }
        }
    };

    private List<ComponentOption> GetCountryOptions()
    {
        var options = new List<ComponentOption>();

        foreach (var regionCode in _phoneNumberUtil.GetSupportedRegions())
        {
            var callingCode = _phoneNumberUtil.GetCountryCodeForRegion(regionCode);
            options.Add(new ComponentOption
            {
                value = regionCode,
                label = regionCode
            });
        }

        return options;
    }




    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var phoneParserInput = JsonSerializer.Deserialize<PhoneParserInput>(json);

        var phoneNumber = phoneParserInput.phone_number;
        var country = phoneParserInput.country;

        try
        {
            var number = _phoneNumberUtil.Parse(phoneNumber, country);

            var result = new Dictionary<string, object>
            {
                { "country_code", country },
                { "country_name", GetCountryName(country) },
                { "is_valid", _phoneNumberUtil.IsValidNumber(number) ? "Yes" : "No" },
                { "is_possible", _phoneNumberUtil.IsPossibleNumber(number) ? "Yes" : "No" },
                { "type", _phoneNumberUtil.GetNumberType(number).ToString() },
                { "international_format", _phoneNumberUtil.Format(number, PhoneNumberFormat.INTERNATIONAL) },
                { "national_format", _phoneNumberUtil.Format(number, PhoneNumberFormat.NATIONAL) },
                { "e164_format", _phoneNumberUtil.Format(number, PhoneNumberFormat.E164) },
                { "rfc3966_format", _phoneNumberUtil.Format(number, PhoneNumberFormat.RFC3966) }
            };

            return result;
        }
        catch (NumberParseException)
        {
            return new Dictionary<string, object>
            {
                { "error", "Invalid phone number format." }
            };
        }
    }

    private string GetCountryName(string countryCode)
    {
        try
        {
            var region = new RegionInfo(countryCode);
            return region.EnglishName;
        }
        catch
        {
            return "Unknown";
        }
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
