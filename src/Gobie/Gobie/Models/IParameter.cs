namespace Gobie.Models;

public interface IParameter
{
    public string InitializerLiteral { get; }

    public string InitializerString { get; }

    public string NamePascal { get; }

    public string CsharpTypeName { get; }
}
