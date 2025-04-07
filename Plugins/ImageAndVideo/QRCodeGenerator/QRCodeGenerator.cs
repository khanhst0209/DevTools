using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace QRCodeGenerator;

public class QRCodeGenerator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "QR Code generator";

    public Category Category => Category.ImageVideo;


    public string Description { get; set; } = "Generate and download a QR code for a URL (or just plain text), and customize the background and foreground colors.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"" mr-3="""" h-30px="""" w-30px="""" shrink-0="""" op-50=""""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><rect x=""4"" y=""4"" width=""6"" height=""6"" rx=""1""></rect><path d=""M7 17v.01""></path><rect x=""14"" y=""4"" width=""6"" height=""6"" rx=""1""></rect><path d=""M7 7v.01""></path><rect x=""4"" y=""14"" width=""6"" height=""6"" rx=""1""></rect><path d=""M17 7v.01""></path><path d=""M14 14h3""></path><path d=""M20 14v.01""></path><path d=""M14 14v3""></path><path d=""M14 20h3""></path><path d=""M17 17h3""></path><path d=""M20 17v3""></path></g></svg>";


    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
