using Jab;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;

namespace TypeCode.Wpf.Components;

[ServiceProviderModule]
[Transient(typeof(IInputBoxViewModelFactory), typeof(InputBoxViewModelFactory))]
[Transient(typeof(IOutputBoxViewModelFactory), typeof(OutputBoxViewModelFactory))]
public interface IComponentsModule
{
    
}