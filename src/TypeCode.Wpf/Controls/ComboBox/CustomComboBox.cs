using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace TypeCode.Wpf.Controls.ComboBox;

public class CustomComboBox : Control
{
    public CustomComboBox()
    {
        PreviewMouseLeftButtonUp += (_, _) => IsComboBoxDropDownOpen = true;
        MouseLeave += (_, _) => IsComboBoxDropDownOpen = false;
    }
    
    public static readonly DependencyProperty IsComboBoxDropDownOpenProperty =
        DependencyProperty.Register(
            name: nameof(IsComboBoxDropDownOpen),
            propertyType: typeof(bool),
            ownerType: typeof(CustomComboBox),
            typeMetadata: new FrameworkPropertyMetadata(false)
        );

    public bool IsComboBoxDropDownOpen
    {
        get => (bool)GetValue(IsComboBoxDropDownOpenProperty);
        private set => SetValue(IsComboBoxDropDownOpenProperty, value);
    }

    public static readonly DependencyProperty ComboBoxItemsProperty =
        DependencyProperty.Register(
            name: nameof(ComboBoxItems),
            propertyType: typeof(IEnumerable),
            ownerType: typeof(CustomComboBox),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

    public IEnumerable ComboBoxItems
    {
        get => (IEnumerable)GetValue(ComboBoxItemsProperty);
        set => SetValue(ComboBoxItemsProperty, value);
    }

    public static readonly DependencyProperty SelectedComboBoxItemProperty =
        DependencyProperty.Register(
            name: nameof(SelectedComboBoxItem),
            propertyType: typeof(object),
            ownerType: typeof(CustomComboBox),
            typeMetadata: new FrameworkPropertyMetadata(null)
        );

    public object SelectedComboBoxItem
    {
        get => GetValue(SelectedComboBoxItemProperty);
        set => SetValue(SelectedComboBoxItemProperty, value);
    }
}