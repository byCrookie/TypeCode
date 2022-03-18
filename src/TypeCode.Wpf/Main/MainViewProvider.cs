using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainViewProvider : IMainViewProvider
{
    private readonly MainResult _mainResult;

    public MainViewProvider()
    {
        if (_mainResult is not null)
        {
            return;
        }
        
        var mainView = new MainWindow();
        var navigationFrame = mainView.MainContent.FindName("NavigationFrame");
        var modalFrame = mainView.FindName("ModalFrame");
        var wizardFrame = mainView.FindName("WizardFrame");

        if (navigationFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(navigationFrame)}");
        if (modalFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(modalFrame)}");
        if (wizardFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(wizardFrame)}");

        _mainResult = new MainResult(mainView, mainView.MainContent, mainView.MainSidebar);
    }
    
    public MainWindow MainWindow()
    {
        return _mainResult.MainWindow;
    }

    public MainContentView MainContentView()
    {
        return _mainResult.MainContent;
    }

    public MainSidebarView MainSidebarView()
    {
        return _mainResult.MainSidebar;
    }
}