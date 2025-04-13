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
using DevTools.Application.Dto.Plugins;
using Services.AssemblyManager;
using DevTools.Application.Dto.File;
using DevTools.Application.Exceptions.UploadFile;

namespace DevTools.Services
{
    public class PluginManagerService : IPluginManagerService
    {
        private readonly IPluginManagerRepository _pluginmanagerRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IPluginRepository _pluginRepository;
        private readonly IMapper _mapper;
        private readonly IAssemblyManager _assemblyManager;
        private readonly IConfiguration _configuration;
        private readonly IPluginLoader _pluginLoader;


        public PluginManagerService(
            IPluginManagerRepository pluginmanagerRepository,
            IPluginRepository pluginRepository,
            IMapper mapper,
            IWebHostEnvironment env,
            IAssemblyManager assemblyManager,
            IConfiguration configuration,
            IPluginLoader pluginLoader)
        {
            _pluginmanagerRepository = pluginmanagerRepository;
            _pluginRepository = pluginRepository;
            _mapper = mapper;
            _env = env;
            _assemblyManager = assemblyManager;
            _configuration = configuration;
            _pluginLoader = pluginLoader;
        }


        public async Task<List<PluginsResponeDTO>> GetAllActivePlugin()
        {
            var plugins = await _pluginRepository.GetAllAsync();
            plugins = plugins.Where(x => x.IsActive == true).ToList();
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

        public async Task<List<PluginResponeWithActiveStatusDTO>> GetAllPlugin()
        {
            var plugins = await _pluginRepository.GetAllAsync();
            return _mapper.Map<List<PluginResponeWithActiveStatusDTO>>(plugins);
        }
        public async Task DeletePluginByIdAsync(int pluginId)
        {
            var plugin = await _pluginRepository.GetByIdAsync(pluginId);

            if (plugin == null)
                throw new PluginNotFound(pluginId);



            await _pluginmanagerRepository.RemoveAsync(pluginId);
            await _pluginRepository.RemoveAsync(plugin.Id);
            await Task.Delay(200);
            await _assemblyManager.UnloadAssemblyAsync(plugin.DllPath);
            await Task.Delay(1000);

            string path = Path.GetFullPath(plugin.DllPath);
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Console.WriteLine($"🗑️ Deleted plugin DLL: {path}");
                }
                else
                {
                    Console.WriteLine($"⚠️ File already deleted or not found: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to delete plugin DLL: {path}\n{ex}");
            }
        }

        private async Task CopyFileIntoFolder(IFormFile file, string folderPath)
        {

            var filePath = Path.Combine(folderPath, file.FileName);
            if (File.Exists(filePath))
            {
                Console.WriteLine("Plugin has been existed");

                if (folderPath != _configuration["Resources_Path:SharedLibaryPath"])
                    throw new DllFileExisted(file.FileName);

                return;
            }
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        public async Task UploadLoadPlugin(UploadFileDTO upload)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Add plugin from website:");

            if (upload.DllFile == null)
                throw new PluginDllNotFound();
            var pluginFolder = _configuration["Resources_Path:PluginPath"];
            var sharedLibaryFolder = _configuration["Resources_Path:SharedLibaryPath"];
            Console.WriteLine("Load Plugin:");
            CopyFileIntoFolder(upload.DllFile, pluginFolder);
            var result = await _pluginLoader.LoadPluginFromFile(Path.Combine(pluginFolder, upload.DllFile.FileName));

            if (!result)
            {
                File.Delete(Path.Combine(pluginFolder, upload.DllFile.FileName));
                Console.WriteLine($"🗑️ Deleted plugin DLL: {Path.Combine(pluginFolder, upload.DllFile.FileName)}, because wrong format");
                throw new PluginUploadFailed(upload.DllFile.FileName);
            }
            if (upload.Libaries != null)
            {
                Console.WriteLine("Load Library:");
                foreach (var file in upload.Libaries)
                {
                    CopyFileIntoFolder(file, sharedLibaryFolder);
                }
            }
            Console.WriteLine("==============================================");
        }
    }
}
