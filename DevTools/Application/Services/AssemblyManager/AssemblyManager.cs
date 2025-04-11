using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using DevTools.Application.Services.AssemblyManager;

namespace Services.AssemblyManager
{
    public class AssemblyManager : IAssemblyManager
    {
        private static readonly Dictionary<string, (Assembly Assembly, CustomAssemblyLoadContext Context)> _loaded = new();

        public async Task<Assembly> LoadAssemblyAsync(string path)
        {
            path = Path.GetFullPath(path);

            if (_loaded.ContainsKey(path))
                return _loaded[path].Assembly;

            var context = new CustomAssemblyLoadContext(path);

            // Load bằng MemoryStream để không bị lock
            byte[] assemblyBytes = await File.ReadAllBytesAsync(path);
            using var stream = new MemoryStream(assemblyBytes);
            var assembly = context.LoadFromStream(stream);

            _loaded[path] = (assembly, context);
            Console.WriteLine($"✅ Loaded {Path.GetFileName(path)} (memory)");
            return assembly;
        }


        public async Task UnloadAssemblyAsync(string path)
        {
            path = Path.GetFullPath(path);

            if (_loaded.TryGetValue(path, out var entry))
            {
                if (entry.Context == null)
                {
                    Console.WriteLine($"❌ Cannot unload: Context is null for {path}");
                    return;
                }

                _loaded.Remove(path);
                entry.Context.Unload();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Console.WriteLine($"🧹 Unloaded {Path.GetFileName(path)}");
            }

        }

        public IReadOnlyDictionary<string, Assembly> GetLoadedAssemblies()
        {
            var result = new Dictionary<string, Assembly>();
            foreach (var kvp in _loaded)
                result[kvp.Key] = kvp.Value.Assembly;
            return result;
        }
    }
}
