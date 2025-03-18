// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using Plugins.DevTool;
// namespace Plugins.Manager;

// public static class PluginManager
// {
//     private static List<IDevToolPlugin> _plugins = new();
//     private static int _lastIndex = 0;

//     public static void LoadPlugins()
//     {
//         _plugins.Clear();

//         string pluginFolder = "./Plugins/DevTool_Plugins";
//         if (!Directory.Exists(pluginFolder))
//             Directory.CreateDirectory(pluginFolder);

//         foreach (var dll in Directory.GetFiles(pluginFolder, "*.dll"))
//         {
//             try
//             {
//                 Assembly assembly = Assembly.LoadFrom(dll);
//                 Type[] types = assembly.GetTypes();


//                 var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);
//                 int start_id = 0;
//                 foreach (var type in pluginTypes)
//                 {
//                     Console.WriteLine($"🎯 Đã tìm thấy plugin: {type.FullName}");

//                     if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
//                     {
//                         plugin.id = _lastIndex;
//                         _lastIndex += 1;
//                         _plugins.Add(plugin);
//                         Console.WriteLine($"✅ Loaded plugin: {plugin.Name}");
//                     }
//                 }

//             }

//             catch (Exception ex)
//             {
//                 Console.WriteLine($"❌ Failed to load {dll}: {ex.Message}");
//             }
//         }
//     }

//     public static void AddPlugin(string path)
//     {
//         try
//         {
//             Assembly assembly = Assembly.LoadFrom(path);
//             Type[] types = assembly.GetTypes();


//             var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);
//             foreach (var type in pluginTypes)
//             {
//                 Console.WriteLine($"🎯 Đã tìm thấy plugin mới: {type.FullName}");

//                 // Tạo instance của plugin và thêm vào danh sách
//                 if (Activator.CreateInstance(type) is IDevToolPlugin plugin)
//                 {
//                     plugin.id = _lastIndex;
//                     _lastIndex += 1;
//                     _plugins.Add(plugin);
//                     Console.WriteLine($"✅ Loaded plugin: {plugin.Name}");
//                 }
//             }

//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"❌ Failed to load flugins {path}: {ex.Message}");
//         }
//     }

//     public static void RemovePlugin(string path)
//     {
//         try
//         {
//             Assembly assembly = Assembly.LoadFrom(path);
//             Type[] types = assembly.GetTypes();

//             var pluginTypes = types.Where(t => typeof(IDevToolPlugin).IsAssignableFrom(t) && !t.IsInterface);

//             foreach (var type in pluginTypes)
//             {
//                 var pluginToRemove = _plugins.FirstOrDefault(p => p.GetType() == type);
//                 if (pluginToRemove != null)
//                 {
//                     _plugins.Remove(pluginToRemove);
//                     Console.WriteLine($"🗑️ Đã xóa plugin: {pluginToRemove.Name}");
//                 }
//                 else
//                 {
//                     Console.WriteLine($"⚠️ Không tìm thấy plugin {type.FullName} trong danh sách.");
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"❌ Lỗi khi xóa plugin: {ex.Message}");
//         }
//     }



//     public static List<IDevToolPlugin> GetPlugins() => _plugins;
// }

