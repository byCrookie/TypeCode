using Framework.Jab.Boot;
using Jab;

namespace TypeCode.Wpf.Application;

[ServiceProviderModule]
[Singleton(typeof(IApplication), typeof(Application))]
public partial interface IAppModule
{
}