﻿namespace Gobie.Models.UserData;

/// <summary>
/// Contains enough data to build the user generator attribute. But not enough to actually run the template.
/// </summary>
public class UserGeneratorAttributeData
{
    private readonly List<RequiredParameter> requiredParameters = new List<RequiredParameter>();

    public UserGeneratorAttributeData(
        ClassIdentifier defintionIdentifier,
        ClassDeclarationSyntax classDeclarationSyntax,
        string attributeBase)
    {
        DefinitionIdentifier = defintionIdentifier;
        AttributeIdentifier = CalculateAttributeIdentifier(defintionIdentifier);
        ClassDeclarationSyntax = classDeclarationSyntax;
        AttributeBase = attributeBase;
    }

    public bool AllowMultiple { get; set; }

    public Compilation? Compilation { get; set; }

    public ClassIdentifier DefinitionIdentifier { get; private set; }

    public ClassIdentifier AttributeIdentifier { get; private set; }

    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

    public List<OptionalParameter> OptionalParameters { get; private set; } = new List<OptionalParameter>();

    public List<GenericParameter> GenericParameters { get; } = new List<GenericParameter>();

    /// <summary>
    /// Required parameters, in the same order as the constructor.
    /// </summary>
    public IEnumerable<RequiredParameter> RequiredParameters =>
        requiredParameters.OrderBy(x => x.RequestedOrder).ThenBy(x => x.DeclaredOrder);

    public string AttributeBase { get; private set; }

    public void AddRequiredParameter(RequiredParameter p) => requiredParameters.Add(p);

    public UserGeneratorAttributeData WithName(string identifier, string? namespaceName)
    {
        namespaceName = string.IsNullOrWhiteSpace(namespaceName) ? AttributeIdentifier.NamespaceName : namespaceName;
        identifier = GenericClassHelpers.AppendAttributeToClassName(identifier);

        AttributeIdentifier = new ClassIdentifier(namespaceName!, identifier);

        return this;
    }

    private static ClassIdentifier CalculateAttributeIdentifier(ClassIdentifier defintionIdentifier)
    {
        const string Generator = "Generator";
        const string Attribute = "Attribute";

        var defName = defintionIdentifier.ClassName;
        var attName = defName;

        if (defName.EndsWith(Generator, StringComparison.OrdinalIgnoreCase))
        {
            attName = defName.Substring(0, defName.Length - Generator.Length);
        }
        attName += Attribute;

        return new ClassIdentifier(defintionIdentifier.NamespaceName, attName);
    }
}
