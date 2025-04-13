using System.Reflection;
using System.Runtime.Loader;

namespace DevTools.Application.Services.AssemblyManager
{
    public class ToolAssemblyWrapper
    {
        public Assembly Assembly { get; }
        public AssemblyLoadContext LoadContext { get; }

        public ToolAssemblyWrapper(Assembly assembly, AssemblyLoadContext context)
        {
            Assembly = assembly;
            LoadContext = context;
        }

        public void Unload()
        {
            LoadContext.Unload();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}