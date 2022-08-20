using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Event
{
	public interface IApplySuggestionEventHandler : IAsyncEventHandler<ApplySuggestionEvent>
	{
		void Initialize(ApplySuggestionEventHandlerParameter parameter);
		void Dispose();
	}
}