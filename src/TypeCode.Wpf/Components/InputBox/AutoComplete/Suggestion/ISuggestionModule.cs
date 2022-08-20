using Jab;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Open;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion
{
	[ServiceProviderModule]
	[Import(typeof(IApplyModule))]
	[Transient(typeof(IBuildSuggestionEventHandler), typeof(BuildSuggestionEventHandler))]
	[Transient(typeof(ISuggestionBuilder), typeof(SuggestionBuilder))]
	public interface ISuggestionModule
	{
	}
}