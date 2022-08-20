namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Open
{
	public class BuildSuggestionEvent
	{
		public BuildSuggestionEvent(IEnumerable<Type> matchingTypes, InputBoxViewModel viewModel)
		{
			MatchingTypes = matchingTypes;
			ViewModel = viewModel;
		}

		public IEnumerable<Type> MatchingTypes { get; }
		public InputBoxViewModel ViewModel { get; }
	}
}