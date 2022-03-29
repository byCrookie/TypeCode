using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.SearchBox;

public class SearchBoxViewModelFactory : ISearchBoxViewModelFactory
{
    private readonly IEventAggregator _eventAggregator;

    public SearchBoxViewModelFactory(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }
    
    public SearchBoxViewModel Create(Func<bool, string?, Task> searchAsync)
    {
        return new SearchBoxViewModel(_eventAggregator, searchAsync);
    }
}