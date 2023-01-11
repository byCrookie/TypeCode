using Jab;
using TypeCode.Wpf.Helper.ViewModels.Flag;

namespace TypeCode.Wpf.Helper.ViewModels;

[ServiceProviderModule]
[Transient(typeof(IFlagScopeFactory), typeof(FlagScopeFactory))]
public interface IViewModelModule
{
}