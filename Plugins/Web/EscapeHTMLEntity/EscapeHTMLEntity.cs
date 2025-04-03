using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace EscapeHTMLEntity;

public class EscapeHTMLEntity : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Escape HTML entities";

    public Category Category => Category.Web;


    public string Description { get; set; } = "Encode text to URL-encoded format (also known as\"percent-encoded\"), or decode from it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M7 8l-4 4l4 4""></path>
            <path d=""M17 8l4 4l-4 4""></path>
            <path d=""M14 4l-4 16""></path>
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
