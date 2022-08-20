using Jab;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete;

[ServiceProviderModule]
[Import(typeof(ISuggestionModule))]
public interface IAutoCompleteModule
{
    
}