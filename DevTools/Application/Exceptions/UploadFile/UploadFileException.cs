namespace DevTools.Application.Exceptions.UploadFile
{
    public class PluginDllNotFound : Exception
    {
        public PluginDllNotFound() : base("Dll file for Plugin is not existed") { }
    }

    public class DllFileExisted : Exception
    {
        public DllFileExisted(string fileName) : base($"Dll file : {fileName} has been existed in server!!!") { }
    }

    public class PluginUploadFailed : Exception
    {
        public PluginUploadFailed(string filename) : base("Failed to load plugin DLL, please check the structure of file") { }
    }
}