namespace TypeCode.Wpf.Main.Content;

public sealed class BannerOpenEvent
{
    public string? Message { get; set; }
    public bool IsLink { get; set; }
    public string? Link { get; set; }
    public TimeSpan? VisibleTime { get; set; }
}