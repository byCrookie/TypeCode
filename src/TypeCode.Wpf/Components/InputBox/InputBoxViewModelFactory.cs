using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox;

public class InputBoxViewModelFactory : IInputBoxViewModelFactory
{
    private readonly IEventAggregator _eventAggregator;

    public InputBoxViewModelFactory(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }
    
    public InputBoxViewModel Create(InputBoxViewModelParameter parameter)
    {
        return new InputBoxViewModel(_eventAggregator, parameter);
    }
}