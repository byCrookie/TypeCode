using Framework.Boot.Autofac.ModuleCatalog;

namespace TypeCode.Console
{
    public class TypeCodeConsoleModuleCatalog : ModuleCatalog
    {
        public TypeCodeConsoleModuleCatalog()
        {
            AddRootModule(new TypeCodeConsoleModule());
        }
    }
}