using Framework.Boot.Autofac;

namespace TypeCode.Business.Modules
{
	public class TypeCodeBusinessModuleCatalog : ModuleCatalog
	{
		public TypeCodeBusinessModuleCatalog()
		{
			AddRootModule(new TypeCodeBusinessModule());
		}
	}
}
