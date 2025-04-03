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
    public string Icon { get; set; } = @"
    <svg xmlns=""http://www.w3.org/2000/svg"" height=""24px"" viewBox=""0 -960 960 960"" width=""24px"" fill=""#e3e3e3"">
        <path d=""M240-80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h40v-80q0-83 58.5-141.5T480-920q83 0 141.5 58.5T680-720v80h40q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Zm0-80h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM360-640h240v-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80ZM240-160v-400 400Z""/>
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
