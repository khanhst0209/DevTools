using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Services.AssemblyManager
{
    public class AssemblyManager : AssemblyLoadContext, IAssemblyManager
    {
        private static readonly Dictionary<string, Assembly> _loadedAssemblies = new();

        public AssemblyManager() : base(isCollectible: true) { }

        public async Task<Assembly> LoadAssemblyAsync(string path)
        {
            if (_loadedAssemblies.ContainsKey(path))
                return _loadedAssemblies[path];

            var assembly = Assembly.LoadFrom(path);
            _loadedAssemblies[path] = assembly;
            Console.WriteLine($"✅ Loaded assembly: {Path.GetFileName(path)}");
            return assembly;

        }

        public async Task UnloadAssemblyAsync(string path)
        {
            if (_loadedAssemblies.ContainsKey(path))
            {
                var context = this;
                Unload();
                _loadedAssemblies.Remove(path);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Console.WriteLine($"🧹 Unloaded assembly: {Path.GetFileName(path)}");
            }
        }

        public IReadOnlyDictionary<string, Assembly> GetLoadedAssemblies() => _loadedAssemblies;

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
