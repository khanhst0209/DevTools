using DevTool.Roles;

namespace DevTools.Dto.Plugins
{
    public class CreatePluginDTO
    {
        public string Name { get; }
        public string Category { get; }
        public string Description { get; set; }
        public Roles AccessiableRole { get; set; }
        public bool IsActive { get; set; }
        public bool IsPremiumTool { get; set; }
    }
}