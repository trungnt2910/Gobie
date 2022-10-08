namespace Gobie.Models;

public class GenericParameter : IParameter
{
    public string InitializerLiteral { get; }

    public string InitializerString { get; }

    public string NamePascal { get; }

    public string CsharpTypeName => "global::System.Type";

    public GenericParameter(string initializerLiteral, string initializerString, string namePascal)
    {
        InitializerLiteral = initializerLiteral;
        InitializerString = initializerString;
        NamePascal = namePascal;
    }
}
