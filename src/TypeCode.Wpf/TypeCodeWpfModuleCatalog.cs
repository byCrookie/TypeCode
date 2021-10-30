using Framework.Boot.Autofac.ModuleCatalog;

namespace TypeCode.Wpf
{
	internal class TypeCodeWpfModuleCatalog : ModuleCatalog
	{
		public TypeCodeWpfModuleCatalog()
		{
			AddRootModule(new TypeCodeWpfModule());
		}
	}
}
