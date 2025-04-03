using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace EncodeDecodeURLFormatedstring;

public class EncodeDecodeURLFormatedstring : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Encode/decode URL-formatted strings";

    public Category Category => Category.Web;


    public string Description { get; set; } = "Encode text to URL-encoded format (also known as \"percent-encoded\"), or decode from it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
        <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
            <path d=""M10 14a3.5 3.5 0 0 0 5 0l4-4a3.5 3.5 0 0 0-5-5l-.5.5""></path>
            <path d=""M14 10a3.5 3.5 0 0 0-5 0l-4 4a3.5 3.5 0 0 0 5 5l.5-.5""></path>
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
