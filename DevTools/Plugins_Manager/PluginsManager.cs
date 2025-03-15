using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Plugins.DevTool;
namespace Plugins.Manager;

public static class PluginManager
{
    private static List<IDevToolPlugin> _plugins = new();

    public static void LoadPlugins()
    {
        _plugins.Clear();

        string pluginFolder = "./Plugins/DevTool_Plugins";
        // Console.WriteLine(pluginFolder);
        if (!Directory.Exists(pluginFolder))
            Directory.CreateDirectory(pluginFolder);

        foreach (var dll in Directory.GetFiles(pluginFolder, "*.dll"))
        {
            // Console.WriteLine(dll);
            try
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                Type[] types = assembly.GetTypes();

                // Console.WriteLine($"📦 DLL {dll} có {types.Length} types:");
                // foreach (var type in types)
                // {
                //     Console.WriteLine($"🔍 Kiểm tra type: {type.FullName}");
                //     Console.WriteLine($"👉 Có kế thừa IDevToolPlugin? {typeof(IDevToolPlugin).IsAssignableFrom(type)}");
                // }


                var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);
                foreach (var type in pluginTypes)
                {
                    Console.WriteLine($"🎯 Đã tìm thấy plugin: {type.FullName}");

                    // Tạo instance của plugin và thêm vào danh sách
                    if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
                    {
                        _plugins.Add(plugin);
                        Console.WriteLine($"✅ Loaded plugin: {plugin.Name}");
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to load {dll}: {ex.Message}");
            }
        }
    }

    public static List<IDevToolPlugin> GetPlugins() => _plugins;
}

