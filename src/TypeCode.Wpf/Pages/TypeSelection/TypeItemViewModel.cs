using System.Windows.Controls;

namespace TypeCode.Wpf.Pages.TypeSelection;

public class TypeItemViewModel : ListBoxItem
{
    public TypeItemViewModel(Type type)
    {
        Content = type.FullName ?? $"{type.Namespace}.{type.Name}";
        Type = type;
        IsSelected = false;
    }
    public Type Type { get; set; }
}