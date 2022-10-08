namespace Gobie.Models;
using System;

public class OptionalParameter : IParameter
{
    public OptionalParameter(string name, string csharpTypeName, string initalizerLiteral, string initializerString)
    {
        InitializerLiteral = initalizerLiteral;
        InitializerString = initializerString;
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#144-constants
        // The type specified in a constant declaration shall be sbyte, byte, short, ushort, int,
        // uint, long, ulong, char, float, double, decimal, bool, string, an enum_type, or a reference_type.

        name = name ?? string.Empty;

        CsharpTypeName = csharpTypeName ?? string.Empty;

        NamePascal = name[0].ToString().ToUpperInvariant() + name.Substring(1);
    }

    public string NamePascal { get; }

    public string CsharpTypeName { get; }

    public string InitializerLiteral { get; }

    public string InitializerString { get; }

    public string Initalizer =>
        string.IsNullOrWhiteSpace(InitializerLiteral) ? string.Empty : $" = {InitializerLiteral};";

    public string PropertyString => $"public {CsharpTypeName} {NamePascal} {{ get; set; }}{Initalizer}";
}
