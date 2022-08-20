namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item
{
	public class SuggestionItem
	{
		public SuggestionItem(string? suggestion)
		{
			Suggestion = suggestion;
		}
		
		public string? Suggestion { get; }
	}
}