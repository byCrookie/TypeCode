using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TypeCode.Wpf.Helper.Behaviours;

public static class SourceChangedScrollTopBehaviour
{
    public static readonly DependencyProperty ScrollToTopProperty = DependencyProperty.RegisterAttached(
        "ScrollToTop",
        typeof(bool),
        typeof(SourceChangedScrollTopBehaviour),
        new UIPropertyMetadata(false, OnScrollToTopPropertyChanged));

    public static bool GetScrollToTop(DependencyObject obj)
    {
        return (bool)obj.GetValue(ScrollToTopProperty);
    }

    public static void SetScrollToTop(DependencyObject obj, bool value)
    {
        obj.SetValue(ScrollToTopProperty, value);
    }

    private static void OnScrollToTopPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
    {
        if (dpo is ItemsControl itemsControl)
        {
            var dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
            if (dependencyPropertyDescriptor != null)
            {
                if ((bool)e.NewValue)
                {
                    dependencyPropertyDescriptor.AddValueChanged(itemsControl, ItemsSourceChanged);
                }
                else
                {
                    dependencyPropertyDescriptor.RemoveValueChanged(itemsControl, ItemsSourceChanged);
                }
            }
        }
    }

    private static void ItemsSourceChanged(object? sender, EventArgs e)
    {
        if (sender is ItemsControl itemsControl)
        {
            EventHandler? eventHandler = null;
            eventHandler = delegate
            {
                if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    var scrollViewer = GetVisualChild<ScrollViewer>(itemsControl);
                    scrollViewer?.ScrollToTop();
                    itemsControl.ItemContainerGenerator.StatusChanged -= eventHandler;
                }
            };
            itemsControl.ItemContainerGenerator.StatusChanged += eventHandler;
        }
    }

    private static T? GetVisualChild<T>(DependencyObject parent) where T : Visual
    {
        var child = default(T);
        var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < numVisuals; i++)
        {
            var v = (Visual)VisualTreeHelper.GetChild(parent, i);
            child = v as T ?? GetVisualChild<T>(v);
            if (child != null)
            {
                break;
            }
        }

        return child;
    }
}