using Jab;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Command;
using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Event;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply
{
	[ServiceProviderModule]
	[Transient(typeof(IApplySuggestionCommandExecutor), typeof(ApplySuggestionCommandExecutor))]
	[Transient(typeof(IApplySuggestionEventHandler), typeof(ApplySuggestionEventHandler))]
	public interface IApplyModule
	{
	}
}