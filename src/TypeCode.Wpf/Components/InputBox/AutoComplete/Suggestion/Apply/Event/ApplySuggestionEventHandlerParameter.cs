namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Event
{
	public class ApplySuggestionEventHandlerParameter
	{
		public ApplySuggestionEventHandlerParameter(InputBoxViewModel viewModel)
		{
			ViewModel = viewModel;
		}

		public InputBoxViewModel ViewModel { get; }
	}
}