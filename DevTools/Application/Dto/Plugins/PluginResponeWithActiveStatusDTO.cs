using DevTools.Dto.Plugins;

namespace DevTools.Application.Dto.Plugins
{
    public class PluginResponeWithActiveStatusDTO : PluginsResponeDTO
    {
        public bool IsActive { get; set; }
    }
}