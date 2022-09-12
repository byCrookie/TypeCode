namespace TypeCode.Wpf.Components.InfoLink;

public sealed class InfoLinkViewModelParameter
{
    public InfoLinkViewModelParameter(string link)
    {
        Link = link;
    }
    
    public string Link { get; }
}