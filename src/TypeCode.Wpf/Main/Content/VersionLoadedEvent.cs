namespace TypeCode.Wpf.Main.Content;

public class VersionLoadedEvent
{
    public VersionLoadedEvent(string currentVersion)
    {
        CurrentVersion = currentVersion;
    }
    
    public string CurrentVersion { get; }
}