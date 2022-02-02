namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class NavigationJournalEntry
{
    public NavigationJournalEntry(InstanceResult newInstanceResult)
    {
        InstanceResult = newInstanceResult;
    }

    public InstanceResult InstanceResult { get; }
    public bool IsOpen { get; set; }
}