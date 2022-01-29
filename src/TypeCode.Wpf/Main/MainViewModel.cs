using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main
{
    public class MainViewModel
    {
        public MainViewModel(
            IFactory<MainContentViewModel> contentViewModelFactory,
            IFactory<MainSidebarViewModel> sidebarViewModelFactory
        )
        {
            MainContentViewModel = contentViewModelFactory.Create();
            MainSidebarViewModel = sidebarViewModelFactory.Create();
        }

        public MainContentViewModel MainContentViewModel { get; set; }
        public MainSidebarViewModel MainSidebarViewModel { get; set; }
    }
}