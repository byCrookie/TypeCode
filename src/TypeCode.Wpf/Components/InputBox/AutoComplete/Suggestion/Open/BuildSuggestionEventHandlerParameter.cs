namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Open
{
	public class BuildSuggestionEventHandlerParameter
	{
		public BuildSuggestionEventHandlerParameter(InputBoxViewModel viewModel)
		{
			ViewModel = viewModel;
		}

		public InputBoxViewModel ViewModel { get; }
	}
}