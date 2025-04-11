using System.Reflection;
using System.Runtime.Loader;

namespace DevTools.Application.Services.AssemblyManager
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public CustomAssemblyLoadContext(string mainAssemblyToLoad) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(mainAssemblyToLoad);
            this.Resolving += ResolveMissingAssembly;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Ưu tiên dùng resolver chính thức
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        private Assembly ResolveMissingAssembly(AssemblyLoadContext context, AssemblyName name)
        {
            Console.WriteLine($"🔍 Trying to resolve: {name.Name}");

            // Tìm thư viện trong các thư mục phụ trợ
            string[] _fallbackDirs = new[]
            {
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Domain", "Plugins", "SharedLibrary"))
            };

            foreach (var dir in _fallbackDirs)
            {
                string possiblePath = Path.Combine(dir, $"{name.Name}.dll");
                Console.WriteLine($"🧭 Looking for {possiblePath}");
                if (File.Exists(possiblePath))
                {
                    Console.WriteLine($"📦 Resolved {name.Name} from {dir}");
                    return context.LoadFromAssemblyPath(possiblePath);
                }
            }

            Console.WriteLine($"❌ Failed to resolve {name.Name}");
            return null;
        }
    }
}


