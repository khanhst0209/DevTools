using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Temperature_Converter;

public class Temperature_Converter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Temperature converter";

    public Category Category => Category.Measurement;


    public string Description { get; set; } = "Degrees temperature conversions for Kelvin, Celsius, Fahrenheit, Rankine, Delisle, Newton, Réaumur, and Rømer.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M10 13.5a4 4 0 1 0 4 0V5a2 2 0 0 0-4 0v8.5""></path><path d=""M10 9h4""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
