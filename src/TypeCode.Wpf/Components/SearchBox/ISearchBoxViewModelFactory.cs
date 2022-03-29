namespace TypeCode.Wpf.Components.SearchBox;

public interface ISearchBoxViewModelFactory
{
    SearchBoxViewModel? Create(Func<bool, string?, Task> searchAsync);
}