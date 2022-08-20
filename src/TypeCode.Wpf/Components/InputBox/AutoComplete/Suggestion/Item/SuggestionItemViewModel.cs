using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item
{
	public partial class SuggestionItemViewModel : ObservableObject
	{
		public SuggestionItemViewModel(SuggestionItem suggestionItem)
		{
			Suggestion = suggestionItem.Suggestion;
		}

		[ObservableProperty]
		private string? _suggestion;
	}
}