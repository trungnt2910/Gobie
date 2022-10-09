namespace Gobie.Models;

/// <summary>
/// Represents an additional class not present in the source.
/// </summary>
public class AdditionalClass
{
    public ClassDeclarationSyntax ClassDeclaration { get; }
    public SemanticModel SemanticModel { get; }

    public AdditionalClass(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
    {
        ClassDeclaration = classDeclaration;
        SemanticModel = semanticModel;
    }
}
