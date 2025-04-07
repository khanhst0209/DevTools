using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace String_Obfuscator;

public class String_Obfuscator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "String obfuscator";

    public Category Category => Category.Text;


    public string Description { get; set; } = "Obfuscate a string (like a secret, an IBAN, or a token) to make it shareable and identifiable without revealing its content.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M3 3l18 18""></path><path d=""M10.584 10.587a2 2 0 0 0 2.828 2.83""></path><path d=""M9.363 5.365A9.466 9.466 0 0 1 12 5c4 0 7.333 2.333 10 7c-.778 1.361-1.612 2.524-2.503 3.488m-2.14 1.861C15.726 18.449 13.942 19 12 19c-4 0-7.333-2.333-10-7c1.369-2.395 2.913-4.175 4.632-5.341""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
