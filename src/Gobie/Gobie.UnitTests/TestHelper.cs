﻿namespace Gobie.Tests;

public static class TestHelper
{
    public static Task Verify(string source)
    {
        // Parse the provided string into a C# syntax tree
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
        // Create references for assemblies we require We could add multiple references if required
        var references = AppDomain.CurrentDomain.GetAssemblies()
                       .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
                       .Select(_ => MetadataReference.CreateFromFile(_.Location))
                       .Concat(new[] { MetadataReference.CreateFromFile(typeof(GobieGenerator).Assembly.Location) })
                       .Concat(new[] { MetadataReference.CreateFromFile(typeof(GobieGeneratorBase).Assembly.Location) })
                       .OrderBy(x => x.FilePath);

        // Create a Roslyn compilation for the syntax tree.
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { syntaxTree },
            references: references);

        // Create an instance of our EnumGenerator incremental source generator
        var generator = new GobieGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the source generator!
        driver = driver.RunGenerators(compilation);

        // Use verify to snapshot test the source generator output!
        return Verifier
            .Verify(driver).UseDirectory("Snapshots");
    }
}
