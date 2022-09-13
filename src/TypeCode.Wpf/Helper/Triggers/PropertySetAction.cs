using System.Reflection;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace TypeCode.Wpf.Helper.Triggers;

public sealed class SetPropertyActionTrigger : TriggerAction<FrameworkElement>
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
    
    public string? IfTargetPropertyIsNullOrEmptyPropertyName
    {
        get => (string?)GetValue(IfTargetPropertyIsNullOrEmptyPropertyNameProperty);
        set => SetValue(IfTargetPropertyIsNullOrEmptyPropertyNameProperty, value);
    }

    public static readonly DependencyProperty IfTargetPropertyIsNullOrEmptyPropertyNameProperty
        = DependencyProperty.Register(nameof(IfTargetPropertyIsNullOrEmptyPropertyName), typeof(string),
            typeof(SetPropertyActionTrigger));
    
    public string? IfTargetPropertyIsNotNullOrEmptyPropertyName
    {
        get => (string?)GetValue(IfTargetPropertyIsNotNullOrEmptyPropertyNameProperty);
        set => SetValue(IfTargetPropertyIsNotNullOrEmptyPropertyNameProperty, value);
    }

    public static readonly DependencyProperty IfTargetPropertyIsNotNullOrEmptyPropertyNameProperty
        = DependencyProperty.Register(nameof(IfTargetPropertyIsNotNullOrEmptyPropertyName), typeof(string),
            typeof(SetPropertyActionTrigger));
    
    public string? IfPropertyIsNullOrEmptyPropertyName
    {
        get => (string?)GetValue(IfPropertyIsNullOrEmptyPropertyNameProperty);
        set => SetValue(IfPropertyIsNullOrEmptyPropertyNameProperty, value);
    }

    public static readonly DependencyProperty IfPropertyIsNullOrEmptyPropertyNameProperty
        = DependencyProperty.Register(nameof(IfPropertyIsNullOrEmptyPropertyName), typeof(string),
            typeof(SetPropertyActionTrigger));
    
    public string? IfPropertyIsNotNullOrEmptyPropertyName
    {
        get => (string?)GetValue(IfPropertyIsNotNullOrEmptyPropertyNameProperty);
        set => SetValue(IfPropertyIsNotNullOrEmptyPropertyNameProperty, value);
    }

    public static readonly DependencyProperty IfPropertyIsNotNullOrEmptyPropertyNameProperty
        = DependencyProperty.Register(nameof(IfPropertyIsNotNullOrEmptyPropertyName), typeof(string),
            typeof(SetPropertyActionTrigger));

    protected override void Invoke(object parameter)
    {
        if (IfTargetPropertyIsNullOrEmptyPropertyName is not null)
        {
            var nullOrEmptyPropertyInfo = TargetObject.GetType().GetProperty(
                IfTargetPropertyIsNullOrEmptyPropertyName,
                BindingFlags.Instance | BindingFlags.Public
                                      | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            var value = nullOrEmptyPropertyInfo?.GetValue(TargetObject);
            if (value is not string stringValue || !string.IsNullOrEmpty(stringValue))
            {
                return;
            }
        }
        
        if (IfTargetPropertyIsNotNullOrEmptyPropertyName is not null)
        {
            var notNullOrEmptyPropertyInfo = TargetObject.GetType().GetProperty(
                IfTargetPropertyIsNotNullOrEmptyPropertyName,
                BindingFlags.Instance | BindingFlags.Public
                                      | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            var value = notNullOrEmptyPropertyInfo?.GetValue(TargetObject);
            if (value is not string stringValue || string.IsNullOrEmpty(stringValue))
            {
                return;
            }
        }
        
        if (IfPropertyIsNullOrEmptyPropertyName is not null)
        {
            var nullOrEmptyPropertyInfo = AssociatedObject.GetType().GetProperty(
                IfPropertyIsNullOrEmptyPropertyName,
                BindingFlags.Instance | BindingFlags.Public
                                      | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            var value = nullOrEmptyPropertyInfo?.GetValue(AssociatedObject);
            if (value is not string stringValue || !string.IsNullOrEmpty(stringValue))
            {
                return;
            }
        }
        
        if (IfPropertyIsNotNullOrEmptyPropertyName is not null)
        {
            var notNullOrEmptyPropertyInfo = AssociatedObject.GetType().GetProperty(
                IfPropertyIsNotNullOrEmptyPropertyName,
                BindingFlags.Instance | BindingFlags.Public
                                      | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

            var value = notNullOrEmptyPropertyInfo?.GetValue(AssociatedObject);
            if (value is not string stringValue || string.IsNullOrEmpty(stringValue))
            {
                return;
            }
        }
        
        var propertyInfo = TargetObject.GetType().GetProperty(
            PropertyName,
            BindingFlags.Instance | BindingFlags.Public
                                  | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

        propertyInfo?.SetValue(TargetObject, PropertyValue);
    }
}