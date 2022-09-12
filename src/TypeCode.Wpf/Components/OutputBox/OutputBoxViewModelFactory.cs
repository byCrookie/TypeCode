namespace TypeCode.Wpf.Components.OutputBox;

public sealed class OutputBoxViewModelFactory : IOutputBoxViewModelFactory
{
    public OutputBoxViewModel Create()
    {
        return new OutputBoxViewModel();
    }
}