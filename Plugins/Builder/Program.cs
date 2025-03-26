using System;
using BuilderSpace;

class Program
{
    static void Main(string[] args)
    {
        Builder builder = new Builder();
        if (args.Length < 2)
        {
            Console.WriteLine("❌ Sai cú pháp! Vui lòng nhập đúng tham số.");
            Console.WriteLine("📌 Cách dùng:");
            Console.WriteLine("   - Build tất cả project: dotnet run -- -all D:\\Projects");
            Console.WriteLine("   - Build từ file: dotnet run -- -file projects.txt");
            return;
        }
        
        if (args[0] == "-all")
        {
            builder.BuildAllProjects(args[1]);
        }
        else if (args[0] == "-file")
        {
            builder.BuildFromFile(args[1]);
        }
    }
}
