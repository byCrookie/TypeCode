using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.Input;

namespace TypeCode.Wpf.Controls.TextBox;

public class CustomTextBox : System.Windows.Controls.TextBox
{
    public CustomTextBox()
    {
        LostFocus += (_, _) => IsAutoCompletionDropDownOpen = false;
    }

    private string? _lastValue;

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
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: false)
        );

    public bool ShowRegex
    {
        get => (bool)GetValue(ShowRegexProperty);
        set => SetValue(ShowRegexProperty, value);
    }

    public static readonly DependencyProperty ShowAutoCompletionProperty =
        DependencyProperty.Register(
            name: nameof(ShowAutoCompletion),
            propertyType: typeof(bool),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: false)
        );

    public bool ShowAutoCompletion
    {
        get => (bool)GetValue(ShowAutoCompletionProperty);
        set => SetValue(ShowAutoCompletionProperty, value);
    }

    public static readonly DependencyProperty IsAutoCompletionDropDownOpenProperty =
        DependencyProperty.Register(
            name: nameof(IsAutoCompletionDropDownOpen),
            propertyType: typeof(bool),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(false)
        );

    public bool IsAutoCompletionDropDownOpen
    {
        get => (bool)GetValue(IsAutoCompletionDropDownOpenProperty);
        private set => SetValue(IsAutoCompletionDropDownOpenProperty, value);
    }

    public static readonly DependencyProperty IsAutoCompletionLoadingProperty =
        DependencyProperty.Register(
            name: nameof(IsAutoCompletionLoading),
            propertyType: typeof(bool),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(false)
        );

    public bool IsAutoCompletionLoading
    {
        get => (bool)GetValue(IsAutoCompletionLoadingProperty);
        private set => SetValue(IsAutoCompletionLoadingProperty, value);
    }

    public static readonly DependencyProperty LoadAutoCompletionAsyncProperty =
        DependencyProperty.Register(
            name: nameof(LoadAutoCompletionAsync),
            propertyType: typeof(Func<string, Task<IEnumerable<string>>>),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(
                new Func<string, Task<IEnumerable<string>>>(_ => Task.FromResult(Enumerable.Empty<string>())))
        );

    public Func<string, Task<IEnumerable<string>>> LoadAutoCompletionAsync
    {
        get => (Func<string, Task<IEnumerable<string>>>)GetValue(LoadAutoCompletionAsyncProperty);
        set => SetValue(LoadAutoCompletionAsyncProperty, value);
    }

    public static readonly DependencyProperty AutoCompletionItemsProperty =
        DependencyProperty.Register(
            name: nameof(AutoCompletionItems),
            propertyType: typeof(ObservableCollection<string>),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(new ObservableCollection<string>(new List<string>
                { "Loading..." }))
        );

    public ObservableCollection<string> AutoCompletionItems
    {
        get => (ObservableCollection<string>)GetValue(AutoCompletionItemsProperty);
        private set => SetValue(AutoCompletionItemsProperty, value);
    }

    public static readonly DependencyProperty ApplyAutoCompletionAsyncProperty =
        DependencyProperty.Register(
            name: nameof(ApplyAutoCompletionAsync),
            propertyType: typeof(Func<string, Task>),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(new Func<string, Task>(_ => Task.CompletedTask),
                PropertyChangedCallback)
        );

    private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        var customTextBox = (CustomTextBox)dependencyObject;

        if (customTextBox.ShowAutoCompletion)
        {
            customTextBox.ApplySelectedAutoCompletionItemCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(
                args => DispatchApplyAutoCompletionAsync(customTextBox, args, (Func<string, Task>)e.NewValue));
        }
    }

    public Func<string, Task> ApplyAutoCompletionAsync
    {
        get => (Func<string, Task>)GetValue(ApplyAutoCompletionAsyncProperty);
        set => SetValue(ApplyAutoCompletionAsyncProperty, value);
    }

    private static async Task DispatchApplyAutoCompletionAsync(CustomTextBox customTextBox,
        SelectionChangedEventArgs? args, Func<string, Task> apply)
    {
        if (!customTextBox.ShowAutoCompletion || args is null || args.AddedItems.Count <= 0)
        {
            return;
        }

        var item = args.AddedItems[0];

        if (item is string itemString)
        {
            await apply(itemString).ConfigureAwait(true);
            customTextBox.IsAutoCompletionDropDownOpen = false;
        }
    }

    public static readonly DependencyProperty ApplySelectedAutoCompletionItemCommandProperty =
        DependencyProperty.Register(
            name: nameof(ApplySelectedAutoCompletionItemCommand),
            propertyType: typeof(ICommand),
            ownerType: typeof(CustomTextBox),
            typeMetadata: new FrameworkPropertyMetadata(default(ICommand))
        );

    public ICommand ApplySelectedAutoCompletionItemCommand
    {
        get => (ICommand)GetValue(ApplySelectedAutoCompletionItemCommandProperty);
        private set => SetValue(ApplySelectedAutoCompletionItemCommandProperty, value);
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (!ShowAutoCompletion)
        {
            return;
        }

        var text = ((System.Windows.Controls.TextBox)e.OriginalSource).Text;

        if (_lastValue == text)
        {
            IsAutoCompletionDropDownOpen = EvaluateIsAutoCompletionDropDownOpen(text);
            return;
        }

        _lastValue = text;

        LoadAndSetAutoCompletionAsync(text).SafeFireAndForget(continueOnCapturedContext: false);
    }

    private async Task LoadAndSetAutoCompletionAsync(string? text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            IsAutoCompletionLoading = true;
            IsAutoCompletionDropDownOpen = EvaluateIsAutoCompletionDropDownOpen(text);

            var items = await LoadAutoCompletionAsync(text).ConfigureAwait(true);

            if (_lastValue == text)
            {
                var itemsList = items.Take(100).ToList();

                if (AutoCompletionItems.Any())
                {
                    AutoCompletionItems.Clear();
                }

                if (itemsList.Any())
                {
                    foreach (var item in itemsList)
                    {
                        AutoCompletionItems.Add(item);
                    }
                }

                IsAutoCompletionLoading = false;
            }
        }

        IsAutoCompletionDropDownOpen = EvaluateIsAutoCompletionDropDownOpen(text);
    }
    
    private bool EvaluateIsAutoCompletionDropDownOpen(string? text)
    {
        return !string.IsNullOrEmpty(text) && AutoCompletionItems.Any() && !string.IsNullOrEmpty(Text);
    }
}