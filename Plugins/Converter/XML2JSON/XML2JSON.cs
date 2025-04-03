using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace XML2JSON;

public class XML2JSON : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "XML to JSON";

    public Category Category => Category.Converter;


    public string Description { get; set; } = "Convert XML to JSON";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M7 4a2 2 0 0 0-2 2v3a2 3 0 0 1-2 3a2 3 0 0 1 2 3v3a2 2 0 0 0 2 2""></path>
            <path d=""M17 4a2 2 0 0 1 2 2v3a2 3 0 0 0 2 3a2 3 0 0 0-2 3v3a2 2 0 0 1-2 2""></path>
        </g>
    </svg>";






    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
