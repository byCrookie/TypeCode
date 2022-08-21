using System.Windows;
using System.Windows.Controls;

namespace TypeCode.Wpf.Controls;

public class CustomTextBox : TextBox
{
    public static readonly DependencyProperty UseRegexProperty =
        DependencyProperty.Register(
            name: nameof(UseRegex),
            propertyType: typeof(bool),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(false)
        );

    public bool UseRegex
    {
        get => (bool)GetValue(UseRegexProperty);
        set => SetValue(UseRegexProperty, value);
    }
    
    public static readonly DependencyProperty ShowRegexProperty =
        DependencyProperty.Register(
            name: nameof(ShowRegex),
            propertyType: typeof(bool),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(false)
        );
    
    public bool ShowRegex
    {
        get => (bool)GetValue(ShowRegexProperty);
        set => SetValue(ShowRegexProperty, value);
    }
}