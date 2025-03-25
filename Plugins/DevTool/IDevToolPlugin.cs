using DevTool.Categories;
using DevTool.Roles;

namespace Plugins.DevTool
{
    public interface IDevToolPlugin
    {
        public int Id { get; set; }
        public string Name { get; }
        public Category Category { get; } // Encode, Generate,...
        public string Description { get; set; }

        public Roles AccessiableRole { get; set; }
        public bool IsActive { get; set; }
        public bool IsPremium { get; set; }

        public string Icon { get; set; }

        public object Execute(object input);


    }
}