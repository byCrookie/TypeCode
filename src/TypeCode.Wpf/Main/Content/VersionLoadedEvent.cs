namespace TypeCode.Wpf.Main.Content;

public sealed class VersionLoadedEvent
{
    public VersionLoadedEvent(string currentVersion)
    {
        CurrentVersion = currentVersion;
    }
    
    public string CurrentVersion { get; }
}