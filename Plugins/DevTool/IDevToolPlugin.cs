namespace Plugins.DevTool
{
    public interface IDevToolPlugin
    {
        public int id {get; }
        public string Name { get; }
        public string Category { get; } // Encode, Generate,...
        public object Execute(object input);
        public void test_function()
        {
            Console.WriteLine("Ditmemay");
        }
    }
}