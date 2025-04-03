using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Lorem_Ipsum_Generator;

public class Lorem_Ipsum_Generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Lorem ipsum generator";

    public Category Category => Category.Text;


    public string Description { get; set; } = "Lorem ipsum is a placeholder text commonly used to demonstrate the visual form of a document or a typeface without relying on meaningful content";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M4 6h16""></path><path d=""M4 12h16""></path><path d=""M4 18h12""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
