using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.TypeSelection;

public partial class TypeSelectionViewModel : ObservableObject, IAsyncNavigatedTo
{
    private readonly IEventAggregator _eventAggregator;

    public TypeSelectionViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        Types = new ObservableCollection<TypeItemViewModel>();
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        var parameter = context.GetParameter<TypeSelectionParameter>();
        Types = new ObservableCollection<TypeItemViewModel>(parameter.Types.Select(t => new TypeItemViewModel(_eventAggregator, t)));
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