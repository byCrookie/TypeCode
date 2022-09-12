using DependencyInjection.Factory;

namespace TypeCode.Wpf.Components.InputBox;

public sealed class InputBoxViewModelFactory : IInputBoxViewModelFactory
{
    private readonly IFactory<InputBoxViewModel> _viewModelFactory;

    public InputBoxViewModelFactory(IFactory<InputBoxViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }
    
    public InputBoxViewModel Create(InputBoxViewModelParameter parameter)
    {
        var viewModel = _viewModelFactory.Create();
        viewModel.Initialize(parameter);
        return viewModel;
    }
}