using Jab;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;

namespace TypeCode.Console.Interactive.Mode.Composer;

[ServiceProviderModule]
[Transient(typeof(IComposerTypeCodeStrategy), typeof(ComposerTypeCodeStrategy))]
[Transient(typeof(IExitOrContinueStep<ComposerContext>), typeof(ExitOrContinueStep<ComposerContext>))]
[Transient(typeof(IMultipleTypeSelectionStep<ComposerContext>), typeof(MultipleTypeSelectionStep<ComposerContext>))]
public interface IComposerModule
{
    
}