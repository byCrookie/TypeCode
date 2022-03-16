using System.Collections.ObjectModel;
using System.Windows.Controls;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.TypeSelection;

public class TypeSelectionViewModel : Reactive, IAsyncNavigatedTo
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

    public SelectionMode SelectionMode { get; set; }

    public ObservableCollection<TypeItemViewModel>? Types
    {
        get => Get<ObservableCollection<TypeItemViewModel>?>();
        set => Set(value);
    }

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