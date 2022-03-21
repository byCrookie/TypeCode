using Framework.Jab;
using Jab;
using TypeCode.Business.Modules;

namespace TypeCode.Console;

[ServiceProvider]
[Import(typeof(IFrameworkModule))]
[Import(typeof(ITypeCodeBusinessModule))]
public partial class TypeCodeConsoleServiceProvider
{
    
}