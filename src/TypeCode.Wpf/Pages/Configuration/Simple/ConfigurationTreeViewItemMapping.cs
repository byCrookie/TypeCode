using System.Windows.Controls;

namespace TypeCode.Wpf.Pages.Configuration.Simple;

public sealed class ConfigurationTreeViewItemMapping<T> where T : class
{
    public ConfigurationTreeViewItemMapping(TreeViewItem item, TreeViewItem parentItem, T type)
    {
        Item = item;
        ParentItem = parentItem;
        Type = type;
    }
    
    public TreeViewItem Item { get; }
    public TreeViewItem ParentItem { get; }
    public T Type { get; }
}