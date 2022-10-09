namespace Gobie;

[Generator]
public partial class GobieGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        Initialize(context, null);
    }
}
