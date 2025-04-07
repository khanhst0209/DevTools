using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace SVGPlaceholderGenerator;

public class SVGPlaceholderGenerator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "SVG placeholder generator";

    public Category Category => Category.ImageVideo;


    public string Description { get; set; } = "Generate svg images to use as a placeholder in your applications.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><path d=""M19 5v14H5V5h14m0-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-4.86 8.86l-3 3.87L9 13.14L6 17h12l-3.86-5.14z"" fill=""currentColor""></path></svg>";






    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
