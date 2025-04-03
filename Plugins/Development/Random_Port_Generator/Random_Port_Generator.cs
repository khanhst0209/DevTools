using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Random_Port_Generator;

public class Random_Port_Generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Random port generator";

    public Category Category => Category.Development;


    public string Description { get; set; } = "Generate random port numbers outside of the range of \"known\" ports (0-1023).";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""3"" y=""4"" width=""18"" height=""8"" rx=""3""></rect><rect x=""3"" y=""12"" width=""18"" height=""8"" rx=""3""></rect><path d=""M7 8v.01""></path><path d=""M7 16v.01""></path></g></svg>";






    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
