using Jab;
using TypeCode.Console.Interactive.Mode.Builder;
using TypeCode.Console.Interactive.Mode.Composer;
using TypeCode.Console.Interactive.Mode.Exit;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.Mapper;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using TypeCode.Console.Interactive.Mode.Selection;
using TypeCode.Console.Interactive.Mode.Specflow;
using TypeCode.Console.Interactive.Mode.UnitTestDependency;

namespace TypeCode.Console.Interactive.Mode;

[ServiceProviderModule]
[Import(typeof(IMapperModule))]
[Import(typeof(IMultipleTypesModule))]
[Import(typeof(ISelectionModule))]
[Import(typeof(IExitOrContinueModule))]
[Import(typeof(ISpecflowModule))]
[Import(typeof(IUnitTestDependencyModule))]
[Import(typeof(IBuilderModule))]
[Import(typeof(IComposerModule))]
[Import(typeof(IExitModule))]
[Transient(typeof(IModeComposer), typeof(ModeComposer))]
public interface IModeModule
{
}