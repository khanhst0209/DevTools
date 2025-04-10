using System.Reflection;
using AutoMapper;
using DevTools.Dto.Category;
using DevTools.Dto.Plugins;
using DevTools.Dto.Querry;
using DevTools.Exceptions.Plugins.PluginsException.cs;
using DevTools.Repositories.Interfaces;
using DevTools.Services.Interfaces;
using Plugins.DevTool;
using DevTool.UISchema;

namespace DevTools.Services
{
    public class PluginManagerService : IPluginManagerService
    {
        private readonly IPluginManagerRepository _pluginmanagerRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IPluginRepository _pluginRepository;
        private readonly IMapper _mapper;


        public PluginManagerService(
            IPluginManagerRepository pluginmanagerRepository,
            IPluginRepository pluginRepository,
            IMapper mapper,
            IWebHostEnvironment env)
        {
  

            _pluginmanagerRepository = pluginmanagerRepository;
            _pluginRepository = pluginRepository;
            _mapper = mapper;
            _env = env;
        }


        public async Task<List<PluginsResponeDTO>> GetAllActivePlugin()
        {
            var plugins = await _pluginRepository.GetAllAsync();
            return _mapper.Map<List<PluginsResponeDTO>>(plugins);
        }

        public async Task<object> Execute(int id, object input)
        {
            var plugin = await _pluginmanagerRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                throw new PluginNotFound(id);
            }
            return plugin.Execute(input);
        }

        public async Task<List<PluginsResponeDTO>> GetAllByQuerry(PluginQuerry querry)
        {
            var plugins = await _pluginRepository.GetAllByQuerryAsync(querry);
            
            return _mapper.Map<List<PluginsResponeDTO>>(plugins);
        }

        public async Task<PluginUI> GetScheme1(int id)
        {
            var plugin = await _pluginmanagerRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                throw new PluginNotFound(id);
            }

            string className = plugin.GetType().Name;
            string directoryPath = Path.Combine(_env.WebRootPath, "Sources", className);

            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
                var fileUrls = new PluginUI();

                foreach (var file in files)
                {
                    // Convert absolute file path to relative URL path
                    string relativePath = file.Replace(_env.WebRootPath, "").Replace("\\", "/");

                    if (file.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                    {
                        fileUrls.html = relativePath;
                    }
                    if (file.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
                    {
                        fileUrls.css = relativePath;
                    }
                    if (file.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
                    {
                        fileUrls.js = relativePath;
                    }
                }

                return fileUrls;
            }

            return null;
        }


        public async Task SetPremiumStatus(int pluginId, bool status)
        {
            var plugin = await _pluginRepository.GetByIdAsync(pluginId);
            if (plugin != null)
            {
                if (plugin.IsPremium != status)
                {
                    plugin.IsPremium = status;
                    await _pluginRepository.UpdateAsync(plugin);
                }

            }
            else
            {
                throw new PluginNotFound(pluginId);
            }
        }

        public async Task SetActiveStatus(int pluginId, bool status)
        {
            var plugin = await _pluginRepository.GetByIdAsync(pluginId);
            if (plugin != null)
            {
                if (plugin.IsActive != status)
                {
                    plugin.IsActive = status;
                    await _pluginRepository.UpdateAsync(plugin);
                }

            }
            else
            {
                throw new PluginNotFound(pluginId);
            }
        }

        public async Task<Schema> GetScheme(int id)
        {
            var plugin = await _pluginmanagerRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                throw new PluginNotFound(id);
            }
            var scheme = plugin.schema;
            
            return scheme;
        }

        public async Task<PluginsResponeDTO> GetPLuginById(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
                throw new PluginNotFound(id);

            return _mapper.Map<PluginsResponeDTO>(plugin);
        }


    }
}
