using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.AssemblyManager
{
    public interface IAssemblyManager
    {
        Task<Assembly> LoadAssemblyAsync(string path);
        Task UnloadAssemblyAsync(string path);
        IReadOnlyDictionary<string, Assembly> GetLoadedAssemblies();
    }
}
