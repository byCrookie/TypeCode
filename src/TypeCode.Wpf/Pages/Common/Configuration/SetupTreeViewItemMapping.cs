using System.Windows.Controls;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class SetupTreeViewItemMapping<T> where T : class
{
    public SetupTreeViewItemMapping(TreeViewItem item, TreeViewItem parentItem, T type)
    {
        Item = item;
        ParentItem = parentItem;
        Type = type;
    }
    
    public TreeViewItem Item { get; }
    public TreeViewItem ParentItem { get; }
    public T Type { get; }
}