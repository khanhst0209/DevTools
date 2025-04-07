using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        string txtFilePath = @"D:\Coding\TKPM\Plugins\hehe\PluginGeneratorTest\test.txt"; // Đường dẫn đến file .txt của bạn
        string solutionFilePath = @"D:\Coding\TKPM\Plugins\hehe\PluginGeneratorTest\PluginGeneratorTest.sln"; // Đường dẫn đến file .sln

        if (!File.Exists(txtFilePath))
        {
            Console.WriteLine("File không tồn tại.");
            return;
        }

        string[] lines = File.ReadAllLines(txtFilePath);
        string currentCategory = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("\t-")) // Làm sạch dòng bắt đầu bằng tab
            {
                var pluginName = line.TrimStart('\t', '-').Trim(); // Tên plugin
                CreateClassLibrary(currentCategory, pluginName); // Tạo Class Library
                UpdateCsproj(currentCategory, pluginName); // Chỉnh sửa .csproj của Class Library
            }
            else if (!string.IsNullOrWhiteSpace(line)) // Là thư mục hoặc category mới
            {
                currentCategory = line.Trim(); // Cập nhật thư mục (category)
                string categoryFolderPath = Path.Combine(Directory.GetCurrentDirectory(), currentCategory);
                // Tạo thư mục cho category chỉ nếu nó chưa tồn tại
                if (!Directory.Exists(categoryFolderPath))
                {
                    Directory.CreateDirectory(categoryFolderPath);
                }
            }
        }

        Console.WriteLine("Đã hoàn thành.");
    }

    // Tạo Class Library
    // Tạo Class Library
    static void CreateClassLibrary(string category, string pluginName)
    {
        string projectFolder = Path.Combine(Directory.GetCurrentDirectory(), category, pluginName);

        if (Directory.Exists(projectFolder))
        {
            Console.WriteLine($"Project {pluginName} đã tồn tại.");
            return;
        }

        // Tạo thư mục cho Class Library
        Directory.CreateDirectory(projectFolder);

        // Tạo project Class Library với tên thư viện trùng với tên plugin
        var startInfo = new ProcessStartInfo("dotnet", $"new classlib -o \"{projectFolder}\" --name \"{pluginName}\"")
        {
            WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), category) // Làm việc trong thư mục category
        };
        var process = Process.Start(startInfo);
        process.WaitForExit();

        // Kiểm tra xem file .csproj có tồn tại không
        string csprojFilePath = Path.Combine(projectFolder, $"{pluginName}.csproj");
        if (!File.Exists(csprojFilePath))
        {
            Console.WriteLine($"File {csprojFilePath} không tồn tại sau khi tạo.");
            return;
        }

        // Đọc và sửa nội dung tệp Class1.cs thành tên lớp trùng với tên plugin
        string classFilePath = Path.Combine(projectFolder, "Class1.cs");
        if (File.Exists(classFilePath))
        {
            // Đổi tên tệp Class1.cs thành tên plugin
            string newClassFilePath = Path.Combine(projectFolder, $"{pluginName}.cs");
            File.Move(classFilePath, newClassFilePath);

            // Đọc nội dung của tệp Class1.cs và thay thế tên lớp
            string classContent = File.ReadAllText(newClassFilePath);
            classContent = classContent.Replace("public class Class1", $"public class {pluginName}");
            File.WriteAllText(newClassFilePath, classContent);

            Console.WriteLine($"Đã thay đổi tên lớp trong {newClassFilePath} thành {pluginName}");
        }

        Console.WriteLine($"Đã tạo class library tại {csprojFilePath} với lớp tên {pluginName} và tệp {pluginName}.cs");
    }


    // Cập nhật .csproj của Class Library
    static void UpdateCsproj(string category, string pluginName)
    {
        string csprojFilePath = Path.Combine(Directory.GetCurrentDirectory(), category, pluginName, $"{pluginName}.csproj");

        if (!File.Exists(csprojFilePath))
        {
            Console.WriteLine($"File {csprojFilePath} không tồn tại.");
            return;
        }

        // Đọc nội dung của .csproj
        string csprojContent = File.ReadAllText(csprojFilePath);

        // Kiểm tra nếu chưa có <ItemGroup> để thêm
        if (!csprojContent.Contains("<ItemGroup>"))
        {
            var projectReference = $@"
<ItemGroup>
    <ProjectReference Include=""..\..\DevTool\DevTool.csproj"" />
</ItemGroup>";
            csprojContent = csprojContent.Replace("</Project>", projectReference + "\n</Project>");
            File.WriteAllText(csprojFilePath, csprojContent);
            Console.WriteLine($"Đã thêm ProjectReference vào {csprojFilePath}");
        }
    }



}
