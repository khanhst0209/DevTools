using DevTool.Roles;

namespace DevTools.Dto.Plugins
{
    public class CreatePluginDTO
    {
        public string Name { get; set;}
        public string Category { get; set;}
        public string Description { get; set; }
        public Roles AccessiableRole { get; set; }
        public bool IsActive { get; set; }
        public bool IsPremiumTool { get; set; }
    }
}