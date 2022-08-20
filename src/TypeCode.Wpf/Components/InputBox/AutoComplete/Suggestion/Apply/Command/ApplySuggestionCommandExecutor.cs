using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Event;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Command
{
	internal class ApplySuggestionCommandExecutor : IApplySuggestionCommandExecutor
	{
		private readonly IEventAggregator _eventAggregator;

		public ApplySuggestionCommandExecutor(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
		}
		
		public Task ExecuteAsync(ApplySuggestionCommandParameter parameter)
		{
			return _eventAggregator.PublishAsync(CreateApplySuggestionEvent(parameter));
		}

		private static ApplySuggestionEvent CreateApplySuggestionEvent(ApplySuggestionCommandParameter parameter)
		{
			return new ApplySuggestionEvent(parameter.SuggestionItem, parameter.ViewModel);
		}
	}
}