using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Open
{
    internal class BuildSuggestionEventHandler : IBuildSuggestionEventHandler
    {
        private BuildSuggestionEventHandlerParameter? _parameter;
        
        private readonly IEventAggregator _eventAggregator;
        private readonly ISuggestionBuilder _suggestionBuilder;

        public BuildSuggestionEventHandler(
            IEventAggregator eventAggregator,
            ISuggestionBuilder suggestionBuilder)
        {
            _eventAggregator = eventAggregator;
            _suggestionBuilder = suggestionBuilder;
        }

        public Task HandleAsync(BuildSuggestionEvent message, CancellationToken? cancellationToken = null)
        {
            if (_parameter!.ViewModel == message.ViewModel)
            {
                var suggestionItems = _suggestionBuilder.Build(message.MatchingTypes);
                _parameter.ViewModel.SuggestionItems.Clear();

                foreach (var suggestionItem in suggestionItems.Select(item => new SuggestionItemViewModel(item)))
                {
                    _parameter.ViewModel.SuggestionItems.Add(suggestionItem);
                }
            }

            return Task.CompletedTask;
        }

        public void Initialize(BuildSuggestionEventHandlerParameter parameter)
        {
            _parameter = parameter;
            _eventAggregator.Subscribe<BuildSuggestionEvent>(this);
        }

        public void Dispose()
        {
            _eventAggregator.Unsubscribe(this);
        }
    }
}