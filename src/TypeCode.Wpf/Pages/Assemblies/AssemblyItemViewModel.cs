using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Assemblies;

public partial class AssemblyItemViewModel : ViewModelBase
{
    public AssemblyItemViewModel(string priority, string assembly, bool ignore)
    {
        Assembly = assembly;
        Ignore = ignore;
        Priority = priority;
    }

    [ObservableProperty]
    private string _priority = null!;
    
    [ObservableProperty]
    private string _assembly = null!;
    
    [ObservableProperty]
    private bool _ignore;
}