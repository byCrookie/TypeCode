using System.Windows.Controls;

namespace TypeCode.Wpf.Pages.Configuration.Simple;

public interface ISimpleConfigurator
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
    Task UpdateAsync(TreeViewItem selectedItem);
    bool CanUpdate(TreeViewItem? selectedItem);
    Task DeleteAsync(TreeViewItem selectedItem);
    bool CanDelete(TreeViewItem? selectedItem);
}