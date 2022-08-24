using System.Windows;
using System.Windows.Controls;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Pages.TypeSelection;

public class TypeItemViewModel : ListBoxItem
{
    private readonly IEventAggregator _eventAggregator;

    public TypeItemViewModel(IEventAggregator eventAggregator, Type type)
    {
        _eventAggregator = eventAggregator;
        Content = type.FullName ?? $"{type.Namespace}.{type.Name}";
        Type = type;
        IsSelected = false;
    }

    protected override void OnSelected(RoutedEventArgs e)
    {
        // _eventAggregator.PublishAsync(new WizardUpdateEvent());
        base.OnSelected(e);
    }

    public Type Type { get; set; }
}