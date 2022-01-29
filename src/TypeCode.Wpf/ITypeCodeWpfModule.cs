using Jab;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Main;
using TypeCode.Wpf.Pages;

namespace TypeCode.Wpf;

[ServiceProviderModule]
[Import(typeof(IAppModule))]
[Import(typeof(IHelperModule))]
[Import(typeof(IMainModule))]
[Import(typeof(IPagesModule))]
public partial interface ITypeCodeWpfModule
{
}