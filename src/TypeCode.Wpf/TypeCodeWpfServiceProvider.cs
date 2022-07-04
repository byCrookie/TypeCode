using Framework;
using Framework.Boot;
using Framework.DependencyInjection;
using Jab;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Components;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Main;
using TypeCode.Wpf.Pages;

namespace TypeCode.Wpf;

[ServiceProvider]
[Import(typeof(IFrameworkModule))]
[Import(typeof(IFrameworkDependencyInjectionModule))]
[Import(typeof(IFrameworkBootModule))]
[Import(typeof(IAppModule))]
[Import(typeof(IComponentsModule))]
[Import(typeof(IHelperModule))]
[Import(typeof(IPagesModule))]
[Import(typeof(IMainModule))]
public partial class TypeCodeWpfServiceProvider
{
}