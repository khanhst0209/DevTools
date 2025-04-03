using System.ComponentModel;

namespace DevTool.Categories
{
    public enum Category
    {
        [Description("Crypto")]
        Crypto,

        [Description("Converter")]
        Converter,

        [Description("Web")]
        Web,

        [Description("Image & Video")]
        ImageVideo,

        [Description("Development")]
        Development
    }
}