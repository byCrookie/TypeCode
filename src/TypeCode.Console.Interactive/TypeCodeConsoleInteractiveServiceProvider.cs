using Framework;
using Framework.Boot;
using Jab;
using TypeCode.Business.Modules;
using TypeCode.Console.Interactive.Modules;

namespace TypeCode.Console.Interactive;

[ServiceProvider]
[Import(typeof(IFrameworkModule))]
[Import(typeof(IFrameworkBootModule))]
[Import(typeof(ITypeCodeBusinessModule))]
[Import(typeof(ITypeCodeConsoleModule))]
public partial class TypeCodeConsoleInteractiveServiceProvider
{
    
}