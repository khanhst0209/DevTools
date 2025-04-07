using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace MAC_address_generator;

public class MAC_address_generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "MAC address generator";

    public Category Category => Category.Network;


    public string Description { get; set; } = "Enter the quantity and prefix. MAC addresses will be generated in your chosen case (uppercase or lowercase)";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""13"" y=""8"" width=""8"" height=""12"" rx=""1""></rect><path d=""M18 8V5a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h9""></path><path d=""M16 9h2""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
