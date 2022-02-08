using Framework.DependencyInjection.Factory;
using TypeCode.Console.Interactive.Mode.Builder;
using TypeCode.Console.Interactive.Mode.Composer;
using TypeCode.Console.Interactive.Mode.Exit;
using TypeCode.Console.Interactive.Mode.Mapper;
using TypeCode.Console.Interactive.Mode.Specflow;
using TypeCode.Console.Interactive.Mode.UnitTestDependency;

namespace TypeCode.Console.Interactive.Mode;

internal class ModeComposer : IModeComposer
{
	private readonly IFactory _factory;

	public ModeComposer(IFactory factory)
	{
		_factory = factory;
	}
		
	public IEnumerable<ITypeCodeStrategy> ComposeOrdered()
	{
		return Compose().OrderBy(mode => mode.Number()).ToList();
	}
		
	private IEnumerable<ITypeCodeStrategy> Compose()
	{
		yield return _factory.Create<ISpecflowTypeCodeStrategy>();
		yield return _factory.Create<IUnitTestDependencyTypeCodeStrategy>();
		yield return _factory.Create<IComposerTypeCodeStrategy>();
		yield return _factory.Create<IMapperTypeCodeStrategy>();
		yield return _factory.Create<IBuilderTypeCodeStrategy>();
		yield return _factory.Create<IExitTypeCodeStrategy>();
	}
}