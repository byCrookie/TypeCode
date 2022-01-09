using Framework.Boot.Autofac;
using TypeCode.Business.Modules;

namespace TypeCode.Console.Modules
{
	internal class TypeCodeConsoleModuleCatalog : ModuleCatalog
	{
		public TypeCodeConsoleModuleCatalog()
		{
			AddRootModule(new TypeCodeBusinessModule());
			AddRootModule(new TypeCodeConsoleModule());
		}
	}
}
