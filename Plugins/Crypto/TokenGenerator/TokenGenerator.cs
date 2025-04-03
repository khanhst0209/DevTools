using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace TokenGenerator;

public class TokenGenerator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Token Generator";

    public Category Category => Category.Crypto;


    public string Description { get; set; } = "Generate random string with the chars you want, uppercase or lowercase letters, numbers and/or symbols.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" height=""24px"" viewBox=""0 -960 960 960"" width=""24px"" fill=""#e3e3e3"">
        <path d=""M280-120 80-320l200-200 57 56-104 104h607v80H233l104 104-57 56Zm400-320-57-56 104-104H120v-80h607L623-784l57-56 200 200-200 200Z""/>
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
