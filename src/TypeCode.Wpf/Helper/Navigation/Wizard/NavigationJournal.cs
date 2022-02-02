namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class NavigationJournal
{
    private readonly List<NavigationJournalEntry> _navigationJournalEntries;
    private int? _currentJournalEntryIndex;

    public NavigationJournal()
    {
        _navigationJournalEntries = new List<NavigationJournalEntry>();
    }

    public NavigationJournalEntry GetOrAddNextEntry(InstanceResult newInstanceResult)
    {
        if (!_currentJournalEntryIndex.HasValue || GetJournalEntriesLastIndex() == _currentJournalEntryIndex)
        {
            var entry = new NavigationJournalEntry(newInstanceResult);
            _navigationJournalEntries.Add(entry);
            _currentJournalEntryIndex = _navigationJournalEntries.IndexOf(entry);
            return entry;
        }

        return _navigationJournalEntries[GetJournalEntriesLastIndex() + 1];
    }

    private int GetJournalEntriesLastIndex()
    {
        if (_navigationJournalEntries.Count > 0)
        {
            return _navigationJournalEntries.Count - 1;
        }

        return 0;
    }
    
    public NavigationJournalEntry? GetBackEntry()
    {
        return CanGoBack() ? _navigationJournalEntries[GetJournalEntriesLastIndex() - 1] : null;
    }
    
    public NavigationJournalEntry? GetCurrentEntry()
    {
        return _currentJournalEntryIndex.HasValue ? _navigationJournalEntries[_currentJournalEntryIndex.Value] : null;
    }

    public bool HasCurrentEntry()
    {
        return _currentJournalEntryIndex.HasValue;
    }

    public bool CanGoBack()
    {
        return _currentJournalEntryIndex is > 0;
    }
}