#r "Libs\\DotLiquid.dll"
#load "Models.csx"

using DotLiquid;
using DotLiquid.NamingConventions;

public sealed class Generator
{
    private readonly Template _template;

    static Generator()
    {
        Template.NamingConvention = new CSharpNamingConvention();
        Template.RegisterFilter(typeof(MyFilter));
        Template.RegisterSafeType(typeof(ParameterObject), new[] { "Name", "Type", "Optional", "Description", "Nullable", "Enum" });
        Template.RegisterSafeType(typeof(MethodObject), new[] { "Name", "Description", "ReturnType", "ReturnDescription", "Parameters" });
        Template.RegisterSafeType(typeof(ControllerObject), new[] { "Name", "Description", "Path", "Methods" });
    }

    public Generator(string template)
    {
        _template = Template.Parse(template);
    }

    public string Render(ControllerObject controller, string ns)
    {
        var data = new
        {
            Namespace = ns,
            Controller = controller
        };
        return _template.Render(Hash.FromAnonymousObject(data));
    }

    public static class MyFilter
    {
        public static string XmlEncode(string input)
        {
            return input
                .Replace("●", "*")
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }        
    }
}
