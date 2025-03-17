using DevTool.Roles;

namespace Plugins.DevTool
{
    public interface IDevToolPlugin
    {
        public int id { get; set; }
        public string Name { get; }
        public string Category { get; } // Encode, Generate,...
        public string Description { get; set; }

        public Roles AccessiableRole { get; set; }
        public bool IsActive { get; set; }
        public bool IsPremiumTool {get; set;}

        public object Execute(object input);

    }
}