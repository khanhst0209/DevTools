using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Text_Statistics;

public class Text_Statistics : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Text statistics";

    public Category Category => Category.Text;


    public string Description { get; set; } = "Get information about a text, the number of characters, the number of words, its size in bytes, ...";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M14 3v4a1 1 0 0 0 1 1h4""></path><path d=""M17 21H7a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h7l5 5v11a2 2 0 0 1-2 2z""></path><path d=""M9 9h1""></path><path d=""M9 13h6""></path><path d=""M9 17h6""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
