using System.Reflection;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace TypeCode.Wpf.Helper.Triggers;

public class SetPropertyActionTrigger : TriggerAction<FrameworkElement>
{
    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    public static readonly DependencyProperty PropertyNameProperty
        = DependencyProperty.Register(nameof(PropertyName), typeof(string),
            typeof(SetPropertyActionTrigger));

    public object PropertyValue
    {
        get => GetValue(PropertyValueProperty);
        set => SetValue(PropertyValueProperty, value);
    }

    public static readonly DependencyProperty PropertyValueProperty
        = DependencyProperty.Register(nameof(PropertyValue), typeof(object),
            typeof(SetPropertyActionTrigger));

    public object TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public static readonly DependencyProperty TargetObjectProperty
        = DependencyProperty.Register(nameof(TargetObject), typeof(object),
            typeof(SetPropertyActionTrigger));

    protected override void Invoke(object parameter)
    {
        var propertyInfo = TargetObject.GetType().GetProperty(
            PropertyName,
            BindingFlags.Instance | BindingFlags.Public
                                  | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

        propertyInfo?.SetValue(TargetObject, PropertyValue);
    }
}