using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Percentage_Calculator;

public class Percentage_Calculator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Percentage calculator";

    public Category Category => Category.Math;


    public string Description { get; set; } = "Easily calculate percentages from a value to another value, or from a percentage to a value.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><circle cx=""17"" cy=""17"" r=""1""></circle><circle cx=""7"" cy=""7"" r=""1""></circle><path d=""M6 18L18 6""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
