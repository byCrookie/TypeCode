using Jab;
using TypeCode.Console.Mode.Builder;
using TypeCode.Console.Mode.Composer;
using TypeCode.Console.Mode.Exit;
using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.Mapper;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;
using TypeCode.Console.Mode.Specflow;
using TypeCode.Console.Mode.UnitTestDependency;
using Workflow;

namespace TypeCode.Console.Mode;

[ServiceProviderModule]
[Import(typeof(IMultipleTypesModule))]
[Import(typeof(ISelectionModule))]
[Import(typeof(IExitOrContinueModule))]
[Import(typeof(IMapperModule))]
[Transient(typeof(IWorkflowBuilder<SpecflowContext>), typeof(WorkflowBuilder<SpecflowContext>))]
[Transient(typeof(ISpecflowTypeCodeStrategy), typeof(SpecflowTypeCodeStrategy))]
[Transient(typeof(IWorkflowBuilder<BuilderContext>), typeof(WorkflowBuilder<BuilderContext>))]
[Transient(typeof(IBuilderTypeCodeStrategy), typeof(BuilderTypeCodeStrategy))]
[Transient(typeof(IWorkflowBuilder<UnitTestDependencyEvaluationContext>), typeof(WorkflowBuilder<UnitTestDependencyEvaluationContext>))]
[Transient(typeof(IUnitTestDependencyTypeCodeStrategy), typeof(UnitTestDependencyTypeCodeStrategy))]
[Transient(typeof(IWorkflowBuilder<ComposerContext>), typeof(WorkflowBuilder<ComposerContext>))]
[Transient(typeof(IComposerTypeCodeStrategy), typeof(ComposerTypeCodeStrategy))]
[Transient(typeof(IExitTypeCodeStrategy), typeof(ExitTypeCodeStrategy))]
[Transient(typeof(IModeComposer), typeof(ModeComposer))]
internal partial interface IModeModule
{
}