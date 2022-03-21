using Jab;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using TypeCode.Console.Interactive.Mode.Selection;

namespace TypeCode.Console.Interactive.Mode.UnitTestDependency;

[ServiceProviderModule]
[Transient(typeof(IUnitTestDependencyTypeCodeStrategy), typeof(UnitTestDependencyTypeCodeStrategy))]
[Transient(typeof(IExitOrContinueStep<UnitTestDependencyEvaluationContext>), typeof(ExitOrContinueStep<UnitTestDependencyEvaluationContext>))]
[Transient(typeof(IMultipleTypeSelectionStep<UnitTestDependencyEvaluationContext>), typeof(MultipleTypeSelectionStep<UnitTestDependencyEvaluationContext>))]
[Transient(typeof(ISelectionStep<UnitTestDependencyEvaluationContext, SelectionStepOptions>), typeof(SelectionStep<UnitTestDependencyEvaluationContext, SelectionStepOptions>))]
public interface IUnitTestDependencyModule
{
    
}