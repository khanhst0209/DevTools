using DevTool.Categories;
using DevTool.Roles;
using DevTool.UISchema;

namespace Plugins.DevTool
{
    public interface IDevToolPlugin
    {
        public object Execute(object input);
        public int Id { get; set; }
        public string Name { get; }
        public Category Category { get; } // Encode, Generate,...
        public string Description { get; set; }

        public Roles AccessiableRole { get; set; }
        public bool IsActive { get; set; }
        public bool IsPremium { get; set; }
        public string Icon { get; set; }

        public Schema schema => null;
        public string GetSheme1();
    }
}