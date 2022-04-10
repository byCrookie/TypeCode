using System.Windows.Controls;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public interface ISetupConfigurator
{
    public Task<TreeViewItem> InitializeAsync();
    public Task AddRootAsync(TreeViewItem parentItem);
    public bool CanAddRoot(TreeViewItem? parentItem);
    public Task AddGroupAsync(TreeViewItem parentItem);
    public bool CanAddGroup(TreeViewItem? parentItem);
    public Task AddPathAsync(TreeViewItem parentItem);
    public bool CanAddPath(TreeViewItem? parentItem);
    public Task AddIncludePatternAsync(TreeViewItem parentItem);
    public bool CanAddIncludePattern(TreeViewItem? parentItem);
    public Task AddSelectorAsync(TreeViewItem parentItem);
    public bool CanAddSelector(TreeViewItem? parentItem);
    public Task ExportAsync();
}