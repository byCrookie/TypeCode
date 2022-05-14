namespace TypeCode.Wpf.Components.InfoLink;

public class InfoLinkViewModelFactory : IInfoLinkViewModelFactory
{
    public InfoLinkViewModel Create(InfoLinkViewModelParameter parameter)
    {
        return new InfoLinkViewModel
        {
            Link = parameter.Link
        };
    }
}