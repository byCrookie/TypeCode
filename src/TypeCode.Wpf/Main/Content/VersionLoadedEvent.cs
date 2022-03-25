namespace TypeCode.Wpf.Main.Content;

public class VersionLoadedEvent
{
    public VersionLoadedEvent(string version)
    {
        Version = version;
    }
    
    public string Version { get; }
}