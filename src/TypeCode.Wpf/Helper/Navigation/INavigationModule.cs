﻿using Jab;
using TypeCode.Wpf.Helper.Navigation.Modal;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;

namespace TypeCode.Wpf.Helper.Navigation;

[ServiceProviderModule]
[Import(typeof(IModalModule))]
[Import(typeof(IWizardModule))]
[Import(typeof(IWizardModule))]
[Import(typeof(IServiceModule))]
[Singleton(typeof(INavigationService), typeof(NavigationService))]
public interface INavigationModule
{
}