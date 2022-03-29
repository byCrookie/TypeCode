namespace TypeCode.Wpf.Components.OutputBox;

public class OutputBoxViewModelFactory : IOutputBoxViewModelFactory
{
    public OutputBoxViewModel Create()
    {
        return new OutputBoxViewModel();
    }
}