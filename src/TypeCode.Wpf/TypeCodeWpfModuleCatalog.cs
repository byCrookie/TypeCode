using Framework.Boot.Autofac;

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
