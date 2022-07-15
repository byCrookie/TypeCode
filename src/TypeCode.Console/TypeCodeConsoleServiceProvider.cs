using Framework;
using Framework.Boot;
using Jab;
using TypeCode.Business.Modules;

namespace TypeCode.Console;

[ServiceProvider]
[Import(typeof(IFrameworkModule))]
[Import(typeof(IFrameworkBootModule))]
[Import(typeof(ITypeCodeBusinessModule))]
internal partial class TypeCodeConsoleServiceProvider
{
    
}