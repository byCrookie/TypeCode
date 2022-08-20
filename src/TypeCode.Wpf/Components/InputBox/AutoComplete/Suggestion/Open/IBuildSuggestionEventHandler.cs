using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Open
{
	public interface IBuildSuggestionEventHandler : IAsyncEventHandler<BuildSuggestionEvent>
	{
		void Initialize(BuildSuggestionEventHandlerParameter parameter);
		void Dispose();
	}
}