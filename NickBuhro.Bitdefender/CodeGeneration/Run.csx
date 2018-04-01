#r "Libs\\Newtonsoft.Json.dll"
#load "Models.csx"
#load "Parser.csx"
#load "Generator.csx"

using System.IO;
using Newtonsoft.Json;

var FullPath = new Func<string, string>(fileName => Path.Combine(Path.GetDirectoryName(ProjectFilePath), fileName));

var objectTree = JsonConvert.DeserializeObject<ControllerObject[]>(
    File.ReadAllText(FullPath("metadata.json")));

new Parser(FullPath(@"..\Bitdefender_ControlCenter_API-Guide_enUS.pdf"))
    .FillControllers(objectTree);

var metadata = JsonConvert.SerializeObject(objectTree, Formatting.Indented);
Output[@"..\metadata.generated.json"]
    .WriteLine(metadata)
    .BuildAction = BuildAction.GenerateOnly;

var ns = "NickBuhro.Bitdefender.Controllers";
var t = File.ReadAllText(FullPath("CodeGeneration/Generator_Controller.liquid"));
var gen = new Generator(t);

foreach (var c in objectTree)
{
    var code = gen.Render(c, ns);
    Output[$"..\\Controllers\\generated\\{c.Name}Controller.cs"]
        .WriteLine(code)
        .BuildAction = BuildAction.Compile;

    Output[$"Logs\\{c.Paragraph} {c.Name}.log"]
        .WriteLine(c.SourceText)
        .BuildAction = BuildAction.GenerateOnly;

    foreach (var m in c.Methods)
    {
        Output[$"Logs\\{m.Paragraph} {m.Name}.log"]
            .WriteLine(m.SourceText)
            .BuildAction = BuildAction.GenerateOnly;

        for (var i = 0; i < m.Parameters.Count; i++)
        {
            var p = m.Parameters[i];
            Output[$"Logs\\{m.Paragraph} {i+1} {p.Name}.log"]
                .WriteLine(p.SourceText)
                .BuildAction = BuildAction.GenerateOnly;
        }
    }
}
