using Jab;
using TypeCode.Wpf.Components.InfoLink;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.InputBox.AutoComplete;
using TypeCode.Wpf.Components.OutputBox;

namespace TypeCode.Wpf.Components;

[ServiceProviderModule]
[Import(typeof(IAutoCompleteModule))]
[Transient(typeof(IInputBoxViewModelFactory), typeof(InputBoxViewModelFactory))]
[Transient(typeof(InputBoxViewModel))]
[Transient(typeof(IOutputBoxViewModelFactory), typeof(OutputBoxViewModelFactory))]
[Transient(typeof(IInfoLinkViewModelFactory), typeof(InfoLinkViewModelFactory))]
public interface IComponentsModule
{
    
}