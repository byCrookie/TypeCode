namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item
{
	internal interface ISuggestionBuilder
	{
		IEnumerable<SuggestionItem> Build(IEnumerable<Type> matchingTypes);
	}
}