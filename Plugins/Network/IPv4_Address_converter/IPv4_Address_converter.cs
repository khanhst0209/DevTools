using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace IPv4_Address_converter;

public class IPv4_Address_converter : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "IPv4 address converter";

    public Category Category => Category.Network;


    public string Description { get; set; } = "Convert an IP address into decimal, binary, hexadecimal, or even an IPv6 representation of it.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M11 10V5h-1m8 14v-5h-1""></path><rect x=""15"" y=""5"" width=""3"" height=""5"" rx=""0.5""></rect><rect x=""10"" y=""14"" width=""3"" height=""5"" rx=""0.5""></rect><path d=""M6 10h.01M6 19h.01""></path></g></svg>";






    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
