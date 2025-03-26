using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuilderSpace;
class Builder
{
    private string pluginsFolder;

    public Builder()
    {
        pluginsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        if (!Directory.Exists(pluginsFolder))
        {
            Directory.CreateDirectory(pluginsFolder);
        }
    }

    public void BuildFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("❌ File không tồn tại!");
            return;
        }

        string[] projectPaths = File.ReadAllLines(filePath);
        foreach (string projectPath in projectPaths)
        {
            Console.WriteLine("===============================================");
            BuildProjectAtPath(projectPath);
            Console.WriteLine("===============================================");
        }
    }

    public void BuildAllProjects(string rootFolder)
    {
        if (!Directory.Exists(rootFolder))
        {
            Console.WriteLine("❌ Thư mục không tồn tại!");
            return;
        }

        string[] projectFolders = Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories)
            .Where(folder => Directory.GetFiles(folder, "*.csproj").Any() && Path.GetFileName(folder) != "Builder") 
            .ToArray();

        foreach (string projectPath in projectFolders)
        {
            Console.WriteLine("===============================================");
            BuildProjectAtPath(projectPath);
            Console.WriteLine("===============================================");
        }
    }

    private void BuildProjectAtPath(string projectPath)
    {
        if (!Directory.Exists(projectPath))
        {
            Console.WriteLine($"❌ Đường dẫn không tồn tại: {projectPath}");
            return;
        }

        Console.WriteLine($"🔍 Kiểm tra project: {projectPath}");

        if (!ContainsValidPluginClassOrInterface(projectPath))
        {
            Console.WriteLine("⚠ Project không kế thừa hoặc implement IDevToolPlugin, cũng không phải interface. Bỏ qua!");
            return;
        }

        Console.WriteLine($"🔹 Đang build project tại: {projectPath}");
        if (BuildProject(projectPath))
        {
            string dllPath = FindDllPath(projectPath);
            if (!string.IsNullOrEmpty(dllPath))
            {
                Console.WriteLine($"✅ Build thành công: {dllPath}");
                CopyToPlugins(dllPath);
            }
            else
            {
                Console.WriteLine("⚠ Không tìm thấy file DLL sau khi build!");
            }
        }
        else
        {
            Console.WriteLine("❌ Build thất bại!");
        }
    }

    private bool BuildProject(string projectPath)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "build --configuration Release",
            WorkingDirectory = projectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = psi })
        {
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }

    private string FindDllPath(string projectPath)
    {
        string binPath = Path.Combine(projectPath, "bin", "Release");
        if (!Directory.Exists(binPath)) return string.Empty;

        string projectName = Path.GetFileName(projectPath);
        string[] netFolders = Directory.GetDirectories(binPath);

        foreach (var folder in netFolders)
        {
            string[] dllFiles = Directory.GetFiles(folder, "*.dll");
            foreach (var dll in dllFiles)
            {
                if (Path.GetFileNameWithoutExtension(dll) == projectName)
                {
                    return dll;
                }
            }
        }
        return string.Empty;
    }

    private void CopyToPlugins(string dllPath)
    {
        string destPath = Path.Combine(pluginsFolder, Path.GetFileName(dllPath));
        File.Copy(dllPath, destPath, true);
        Console.WriteLine($"📂 File DLL đã được sao chép vào: {destPath}");
    }

    private bool ContainsValidPluginClassOrInterface(string projectPath)
    {
        string[] csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);

        foreach (string file in csFiles)
        {
            string content = File.ReadAllText(file);

            // Kiểm tra xem có class kế thừa hoặc implement IDevToolPlugin không
            bool hasValidClass = Regex.IsMatch(content, @"class\s+\w+\s*:\s*(\w+,\s*)?IDevToolPlugin");

            // Kiểm tra xem có interface IDevToolPlugin không
            bool hasValidInterface = Regex.IsMatch(content, @"interface\s+IDevToolPlugin");

            if (hasValidClass || hasValidInterface)
            {
                return true;
            }
        }

        return false;
    }
}
