﻿using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public interface IWizardRunner
{
    public Task<NavigationContext> RunAsync(Wizard wizard);
}