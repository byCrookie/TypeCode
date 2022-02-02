using System.Windows.Controls;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public class InstanceResult
{
    public InstanceResult(Type viewType, UserControl viewInstance, Type viewModelType, object viewModelInstance)
    {
        ViewType = viewType;
        ViewInstance = viewInstance;
        ViewModelType = viewModelType;
        ViewModelInstance = viewModelInstance;
    }

    public Type ViewType { get; }
    public object ViewInstance { get; }
    public Type ViewModelType { get; }
    public object ViewModelInstance { get; }
}