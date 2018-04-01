using System;
using System.Collections.Generic;
using System.Text;

private static readonly List<string> _enums = new List<string>
{
    "CompanyType"
};

/// <summary>
/// Resolve type name from PDF document.
/// </summary>
/// <param name="name">Type name from PDF document. For example: String, Number, Boolean.</param>
/// <returns>Correct C# type name, that can be used for definition in code. For example: string, int, bool.</returns>
public static string ResolveType(string name)
{
    switch (name)
    {
        case "String":
            return "string";
        case "Number":
            return "int";
        case "Boolean":
            return "bool";
        default:
            throw new ArgumentException($"Type '{name}' is not supported by the method '{nameof(ResolveType)}'.");
    }
}

/// <summary>
/// Determine if the type is nullable (class).
/// </summary>
/// <param name="name">Correct C# type name, that can be used for definition in code. For example: string, int, bool.</param>
/// <returns>True if the type is nullable (class).</returns>
public static bool IsTypeNullable(string name)
{
    if (_enums.Contains(name))
        return false;

    switch (name)
    {
        case "string":
            return true;
        case "int":
        case "bool":
            return false;
        default:
            throw new ArgumentException($"Type '{name}' is not supported by the method '{nameof(IsTypeNullable)}'.");
    }
}

/// <summary>
/// Determine if the type is enumeration (enum).
/// </summary>
/// <param name="name">Type name. For example: "CompanyType".</param>
/// <returns>Trie if the type is enum.</returns>
public static bool IsEnum(string name)
{
    return _enums.Contains(name);
}
