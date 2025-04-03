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
    public string Icon { get; set; } = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 24 24'>
    <g fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>
        <path d='M18 4l3 3l-3 3'></path>
        <path d='M18 20l3-3l-3-3'></path>
        <path d='M3 7h3a5 5 0 0 1 5 5a5 5 0 0 0 5 5h5'></path>
        <path d='M21 7h-5a4.978 4.978 0 0 0-2.998.998M9 16.001A4.984 4.984 0 0 1 6 17H3'></path>
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
