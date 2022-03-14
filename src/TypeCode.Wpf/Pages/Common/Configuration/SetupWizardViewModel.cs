using System.IO;
using System.Windows.Controls;
using System.Xml.Linq;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class SetupWizardViewModel : Reactive, IAsyncInitialNavigated
{
    public async Task OnInititalNavigationAsync(NavigationContext context)
    {
        var cfg = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        var doc = AsDocument(xml);

        if (doc is null)
        {
            throw new Exception("Configuration.cfg.xml has not a valid format");
        }

        BuildTreeView(doc);
    }

    private void BuildTreeView(XDocument doc)
    {
        TreeViewItems = new List<TreeViewItem>();
        BuildTypeCodeConfiguration(doc);
    }

    private void BuildTypeCodeConfiguration(XDocument doc)
    {
        foreach (var typeCodeConfiguration in doc.Elements().Where(e => e.Name == "TypeCodeConfiguration"))
        {
            var typeCodeConfigurationItem = BuildItem(typeCodeConfiguration);

            BuildAssemblyRoots(typeCodeConfiguration, typeCodeConfigurationItem);

            TreeViewItems?.Add(typeCodeConfigurationItem);
        }
    }

    private static void BuildAssemblyRoots(XElement typeCodeConfiguration, TreeViewItem typeCodeConfigurationItem)
    {
        foreach (var assemblyRoot in typeCodeConfiguration.Elements().Where(e => e.Name == "AssemblyRoot"))
        {
            var assemblyRootItem = BuildItem(assemblyRoot);

            BuildIncludeAssemblyPatterns(assemblyRoot, assemblyRootItem);

            BuildAssemblyGroups(assemblyRoot, assemblyRootItem);

            typeCodeConfigurationItem.Items.Add(assemblyRootItem);
        }
    }

    private static void BuildAssemblyGroups(XElement assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var assemblyGroup in assemblyRoot.Elements().Where(e => e.Name == "AssemblyGroup"))
        {
            var assemblyGroupItem = BuildItem(assemblyGroup);

            BuildAssemblyPaths(assemblyGroup, assemblyGroupItem);

            assemblyRootItem.Items.Add(assemblyGroupItem);
        }
    }

    private static void BuildAssemblyPaths(XElement assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPath in assemblyGroup.Elements().Where(e => e.Name == "AssemblyPath"))
        {
            var assemblyPathItem = BuildItem(assemblyPath);

            assemblyGroupItem.Items.Add(assemblyPathItem);
        }
    }

    private static void BuildIncludeAssemblyPatterns(XElement assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var includeAssemblyPattern in assemblyRoot.Elements().Where(e => e.Name == "IncludeAssemblyPattern"))
        {
            var includeAssemblyPatternItem = BuildItem(includeAssemblyPattern);

            assemblyRootItem.Items.Add(includeAssemblyPatternItem);
        }
    }
    
    private static TreeViewItem BuildItem(XElement element)
    {
        var item = new TreeViewItem
        {
            Header = BuildHeader(element),
            IsExpanded = true
        };

        var textNode = element.Nodes().OfType<XText>().FirstOrDefault();
        if (textNode is not null)
        {
            // item.Items.Add(new TreeViewItem
            // {
            //     Header = textNode.Value
            // });

            item.Header = $"{item.Header} {textNode.Value}";
        }

        return item;
    }
    
    private static string BuildHeader(XElement element)
    {
        var attributes = element.Attributes().ToList();
        return $"{element.Name} {string.Join(" ", attributes.Select(a => $"{a.Name}={a.Value}").ToList())}";
    }

    private static XDocument? AsDocument(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return null;
        }
        
        try
        {
            var doc = XDocument.Parse(xml);
            return doc;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public List<TreeViewItem>? TreeViewItems
    {
        get => Get<List<TreeViewItem>?>();
        set => Set(value);
    }
}