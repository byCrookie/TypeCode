using TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Item;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox.AutoComplete.Suggestion.Apply.Event
{
    internal class ApplySuggestionEventHandler : IApplySuggestionEventHandler
    {
        private ApplySuggestionEventHandlerParameter? _parameter;

        private readonly IEventAggregator _eventAggregator;

        public ApplySuggestionEventHandler(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Task HandleAsync(ApplySuggestionEvent message, CancellationToken? cancellationToken = null)
        {
            if (_parameter is null)
            {
                throw new Exception($"{nameof(ApplySuggestionEventHandler)} is not initialized");
            }
            
            if (_parameter.ViewModel == message.ViewModel)
            {
                if (!string.IsNullOrEmpty(message.SuggestionItem.Suggestion))
                {
                    _parameter.ViewModel.Input = message.SuggestionItem.Suggestion;
                }
            }

            return Task.CompletedTask;
        }

        public void Initialize(ApplySuggestionEventHandlerParameter parameter)
        {
            _parameter = parameter;
            _eventAggregator.Subscribe<ApplySuggestionEvent>(this);
        }

        public void Dispose()
        {
            _eventAggregator.Unsubscribe(this);
        }
    }
}