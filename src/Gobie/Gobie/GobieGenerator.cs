﻿namespace Gobie;

public partial class GobieGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValuesProvider<AdditionalClass>? additionalClasses)
    {
        // Find the user templates and report diagnostics on issues.
        var userTemplateSyntaxOrDiagnostics = GeneratorDiscovery.FindUserTemplates(context);
        DiagnosticsReporting.Report(context, userTemplateSyntaxOrDiagnostics);
        var userTemplateSyntax = ExtractData(userTemplateSyntaxOrDiagnostics);

        GeneratorDiscovery.GenerateAttributes(context, userTemplateSyntax);

        var compliationAndGeneratorDeclarations = userTemplateSyntax.Combine(context.CompilationProvider.Select((c, ct) => (Compilation?)c));
        var userGeneratorsOrDiagnostics = GeneratorDiscovery.GetFullGenerators(compliationAndGeneratorDeclarations);
        DiagnosticsReporting.Report(context, userGeneratorsOrDiagnostics);
        var userGenerators = ExtractData(userGeneratorsOrDiagnostics);

        var userGeneratorArray = userGenerators.Collect();

        if (additionalClasses != null)
        {
            var additionalTemplateSyntaxOrDiagnostics = GeneratorDiscovery.FindUserTemplates(additionalClasses.Value);
            DiagnosticsReporting.Report(context, additionalTemplateSyntaxOrDiagnostics);
            var additionalTemplateSyntax = ExtractData(additionalTemplateSyntaxOrDiagnostics);

            GeneratorDiscovery.GenerateAttributes(context, additionalTemplateSyntax);

            var additionalCompliationAndGeneratorDeclarations = additionalTemplateSyntax.Combine(context.CompilationProvider.Select((c, ct) => (Compilation?)null));
            var additionalGeneratorsOrDiagnostics = GeneratorDiscovery.GetFullGenerators(additionalCompliationAndGeneratorDeclarations);
            DiagnosticsReporting.Report(context, additionalGeneratorsOrDiagnostics);
            var additionalGenerators = ExtractData(additionalGeneratorsOrDiagnostics);

            userGeneratorArray = userGenerators.Collect()
                .Combine(additionalGenerators.Collect())
                .Select((arrs, ct) =>
                {
                    var (arr1, arr2) = arrs;
                    return ImmutableArray.Create(arr1.Concat(arr2).ToArray());
                });
        }

        // TODO look for gobie settings coming from attributes
        var assemblyAtt = AssemblyAttributes.FindAssemblyAttributes(context);

        // ========== Target Discovery Workflow ================
        // First: Discover classes and field targets.
        var mwa = TargetDiscovery.FindMembersWithAttributes(context);
        var mwaAndGenerators = mwa.Combine(userGeneratorArray);
        var probableTargets = TargetDiscovery.FindProbableTargets(mwaAndGenerators);
        var compliationAndProbableTargets = probableTargets.Where(x => x is not null).Combine(context.CompilationProvider);
        var targetsOrDiagnostics = TargetDiscovery.GetTargetsOrDiagnostics(compliationAndProbableTargets);
        DiagnosticsReporting.Report(context, targetsOrDiagnostics);
        var memberTargets = ExtractManyData(targetsOrDiagnostics);

        // Second: Discover assembly targets (i.e. Requests for global template gen).
        var assemblyAttributesAndGenerators = assemblyAtt.Combine(userGeneratorArray);
        var probableAssemblyTargets = TargetDiscovery.FindProbableAssemblyTargets(assemblyAttributesAndGenerators);
        var compliationAndProbableAssemblyTargets = probableAssemblyTargets.Where(x => x is not null).Combine(context.CompilationProvider);
        var assemblyTargetsOrDiagnostics = TargetDiscovery.GetAssemblyTargetsOrDiagnostics(compliationAndProbableAssemblyTargets);
        DiagnosticsReporting.Report(context, assemblyTargetsOrDiagnostics);
        var assemblyTargets = ExtractData(assemblyTargetsOrDiagnostics);

        // =========== Code Generation Workflow ==============

        // First: Concatenate the target types into a single output.
        var targets = memberTargets.Collect().Combine(assemblyTargets.Collect());

        // Consolidate outputs down to files and output them.
        var codeOutOrDiagnostics = CodeGeneration.CollectOutputs(targets);
        DiagnosticsReporting.Report(context, codeOutOrDiagnostics);
        var codeOut = codeOutOrDiagnostics.Select(selector: (x, _) => x.Data);
        context.RegisterSourceOutput(codeOut, static (spc, source) => CodeGeneration.Output(spc, source));
    }

    private static IncrementalValuesProvider<T> ExtractData<T>(IncrementalValuesProvider<DataOrDiagnostics<T>> values)
    {
        return values
            .Where(x => x.Data is not null)
            .Select(selector: (s, _) => s.Data!);
    }

    private static IncrementalValuesProvider<T> ExtractManyData<T>(IncrementalValuesProvider<DataOrDiagnostics<ImmutableArray<T>>> values)
    {
        return values
            .SelectMany(selector: (s, _) => s.Data);
    }
}
