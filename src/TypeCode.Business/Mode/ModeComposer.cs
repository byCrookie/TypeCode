using System.Collections.Generic;
using System.Linq;
using Framework.Autofac.Factory;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.Mode.UnitTestDependency;

namespace TypeCode.Business.Mode
{
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
		}
	}
}