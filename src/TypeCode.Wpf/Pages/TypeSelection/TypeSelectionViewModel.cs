using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.TypeSelection;

public sealed partial class TypeSelectionViewModel : ViewModelBase, IAsyncNavigatedTo
{
    public TypeSelectionViewModel()
    {
        Types = new ObservableCollection<TypeItemViewModel>();
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        var parameter = context.GetParameter<TypeSelectionParameter>();
        Types = new ObservableCollection<TypeItemViewModel>(parameter.Types.Select(t => new TypeItemViewModel(t)));
        SelectionMode = parameter.AllowMultiSelection ? SelectionMode.Multiple : SelectionMode.Single;
        return Task.CompletedTask;
    }

    [ObservableProperty]
    private SelectionMode _selectionMode;

    [ObservableProperty]
    [ChildViewModel]
    private ObservableCollection<TypeItemViewModel>? _types;

    public IEnumerable<Type> SelectedTypes
    {
        get
        {
            return Types?
                .Where(t => t.IsSelected)
                .Select(t => t.Type) ?? new List<Type>();
        }
    }
}