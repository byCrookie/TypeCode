namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Command
{
	public interface IApplySuggestionCommandExecutor
	{
		Task ExecuteAsync(ApplySuggestionCommandParameter parameter);
	}
}