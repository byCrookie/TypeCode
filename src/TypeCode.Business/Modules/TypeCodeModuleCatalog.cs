using Framework.Boot.Autofac.ModuleCatalog;

namespace TypeCode.Business.Modules
{
	internal class TypeCodeModuleCatalog : ModuleCatalog
	{
		public TypeCodeModuleCatalog()
		{
			AddRootModule(new TypeCodeModule());
		}
	}
}
