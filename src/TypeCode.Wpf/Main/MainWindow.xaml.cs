using System.Windows;
using System.Windows.Input;

namespace TypeCode.Wpf.Main;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        MinimizeButton.Click += Minimize;
        MaximizeButton.Click += Maximize;
        CloseButton.Click += Close;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Maximize(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Minimize(object sender, RoutedEventArgs routedEventArgs)
    {
        WindowState = WindowState.Minimized;
    }

    private void WindowOnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                var mousePosition = e.GetPosition(this);
                Top = mousePosition.Y - 10;
            }

            DragMove();
        }
    }
}