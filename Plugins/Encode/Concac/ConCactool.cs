using DevTool.Roles;
using Plugins.DevTool;

namespace Concac;

public class ConCac : IDevToolPlugin
{
    public int id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Name => "ConCactool";

    public string Category => "Encode";

    public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Roles AccessiableRole { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsPremiumTool { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public object Execute(object input)
    {
        throw new NotImplementedException();
    }
}
