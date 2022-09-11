namespace TypeCode.Wpf.Helper.ViewModels;

public abstract class ViewModelBase : CommunityToolkit.Mvvm.ComponentModel.ObservableValidator
{
    public new void ValidateAllProperties()
    {
        base.ValidateAllProperties();
    }

    public void OnAllPropertiesChanged()
    {
        OnPropertyChanged(string.Empty);
    }

    public bool Initialized { get; set; }
}