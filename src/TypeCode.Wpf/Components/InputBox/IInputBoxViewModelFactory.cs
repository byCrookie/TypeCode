namespace TypeCode.Wpf.Components.InputBox;

public interface IInputBoxViewModelFactory
{
    InputBoxViewModel Create(InputBoxViewModelParameter parameter);
}