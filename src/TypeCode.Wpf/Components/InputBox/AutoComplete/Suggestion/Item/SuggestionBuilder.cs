namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item
{
    internal class SuggestionBuilder : ISuggestionBuilder
    {
        public IEnumerable<SuggestionItem> Build(IEnumerable<Type> matchingTypes)
        {
            return matchingTypes.Select(matchingType => new SuggestionItem(matchingType.Name));
        }
    }
}