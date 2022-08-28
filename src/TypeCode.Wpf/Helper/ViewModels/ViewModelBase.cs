namespace TypeCode.Wpf.Helper.ViewModels;

public class ViewModelBase : CommunityToolkit.Mvvm.ComponentModel.ObservableValidator
{
    public new void ValidateAllProperties()
    {
        base.ValidateAllProperties();
    }
}