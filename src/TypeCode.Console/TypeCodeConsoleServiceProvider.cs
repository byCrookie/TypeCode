using Framework;
using Framework.Boot;
using Framework.DependencyInjection;
using Jab;
using TypeCode.Business.Modules;

namespace TypeCode.Console;

[ServiceProvider]
[Import(typeof(IFrameworkModule))]
[Import(typeof(IFrameworkDependencyInjectionModule))]
[Import(typeof(IFrameworkBootModule))]
[Import(typeof(ITypeCodeBusinessModule))]
public partial class TypeCodeConsoleServiceProvider
{
    
}