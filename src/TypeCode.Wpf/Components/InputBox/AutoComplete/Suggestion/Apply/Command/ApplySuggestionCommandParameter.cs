using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Command
{
	public class ApplySuggestionCommandParameter
	{
		public ApplySuggestionCommandParameter(SuggestionItem suggestionItem, InputBoxViewModel viewModel)
		{
			SuggestionItem = suggestionItem;
			ViewModel = viewModel;
		}

		public SuggestionItem SuggestionItem { get; }
		public InputBoxViewModel ViewModel { get; }
	}
}