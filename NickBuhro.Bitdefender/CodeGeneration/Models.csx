#r "Libs\\Newtonsoft.Json.dll"
#load "TypeHelper.csx"

using System.Collections.Generic;
using Newtonsoft.Json;

public sealed class ControllerObject
{
    public string Name { get; set; }

    public string Paragraph { get; set; }

    public string Description { get; set; }

    public string Path { get; set; }

    public List<MethodObject> Methods { get; set; }

    [JsonIgnore]
    public string SourceText { get; set; }

    [JsonIgnore]
    public bool Visited { get; set; }

    public override string ToString()
    {
        return Name;
    }
}

public sealed class MethodObject
{
    public string Name { get; set; }

    public string Paragraph { get; set; }

    public string Description { get; set; }

    public List<ParameterObject> Parameters { get; set; }

    public string ReturnType { get; set; }

    public string ReturnDescription { get; set; }

    [JsonIgnore]
    public string SourceText { get; set; }

    [JsonIgnore]
    public bool Visited { get; set; }

    public override string ToString()
    {
        return Name;
    }
}

public sealed class ParameterObject
{
    public string Name { get; set; }

    public string Type { get; set; }

    public bool Optional { get; set; }

    public string Description { get; set; }
    
    [JsonIgnore]
    public string SourceText { get; set; }

    [JsonIgnore]
    public bool Visited { get; set; }

    [JsonIgnore]
    public bool Nullable => IsTypeNullable(Type);

    [JsonIgnore]
    public bool Enum => IsEnum(Type);

    public override string ToString()
    {
        return Name;
    }
}
