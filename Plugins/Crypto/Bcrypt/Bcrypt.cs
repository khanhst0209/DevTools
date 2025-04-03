using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Bcrypt;

public class Bcrypt : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Bcrypt";

    public Category Category => Category.Crypto;


    public string Description { get; set; } = "Hash and compare text string using bcrypt. Bcrypt is a password-hashing function based on the Blowfish cipher.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon {get; set;} = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 24 24'>
    <g fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>
        <rect x='8' y='11' width='8' height='5' rx='1'></rect>
        <path d='M10 11V9a2 2 0 1 1 4 0v2'></path>
        <rect x='4' y='4' width='16' height='16' rx='2'></rect>
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
