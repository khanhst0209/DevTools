using DevTool.Categories;
using DevTool.Roles;
using Plugins.DevTool;

namespace Math_Evaluator;

public class Math_Evaluator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Math evaluator";

    public Category Category => Category.Math;


    public string Description { get; set; } = "A calculator for evaluating mathematical expressions. You can use functions like sqrt, cos, sin, abs, etc.";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M16 13l4 4m0-4l-4 4""></path><path d=""M20 5h-7L9 19l-3-6H4""></path></g></svg>";



    public object Execute(object input)
    {
        throw new NotImplementedException();
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
