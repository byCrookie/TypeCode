using System;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Application.Boot.SetupWpfApplication;

public class WpfWindowProvider : IWpfWindowProvider
{
    private static MainWindow _mainWindow;
    
    public WpfWindowProvider()
    {
        _mainWindow = new MainWindow();
    }
    
    public WpfWindowProviderResult Get()
    {
        var navigationFrame = _mainWindow.MainContent.FindName("NavigationFrame");
        var modalFrame = _mainWindow.FindName("ModalFrame");
        var wizardFrame = _mainWindow.FindName("WizardFrame");

        if (navigationFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(navigationFrame)}");
        if (modalFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(modalFrame)}");
        if (wizardFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(wizardFrame)}");

        return new WpfWindowProviderResult
        {
            MainWindow = _mainWindow,
            MainContentView = _mainWindow.MainContent,
            MainSidebarView = _mainWindow.MainSidebar
        };
    }
}