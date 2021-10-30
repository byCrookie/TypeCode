using Framework.Boot.Autofac.ModuleCatalog;

namespace TypeCode.Business.Modules
{
	internal class TypeCodeBusinessModuleCatalog : ModuleCatalog
	{
		public TypeCodeBusinessModuleCatalog()
		{
			AddRootModule(new TypeCodeBusinessModule());
		}
	}
}
