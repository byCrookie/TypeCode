using System.IO;
using System.Reflection;
using System.Windows.Controls;
using TypeCode.Business.Configuration;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyRoot;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class SetupWizardViewModel : Reactive, IAsyncInitialNavigated
{
    private readonly IWizardNavigationService _wizardNavigationService;
    private readonly IGenericXmlSerializer _genericXmlSerializer;

    public SetupWizardViewModel(IWizardNavigationService wizardNavigationService, IGenericXmlSerializer genericXmlSerializer)
    {
        _wizardNavigationService = wizardNavigationService;
        _genericXmlSerializer = genericXmlSerializer;
    }

    public async Task OnInititalNavigationAsync(NavigationContext context)
    {
        var configuration = await ReadXmlConfigurationAsync().ConfigureAwait(true);
        await BuildTreeViewAsync(configuration).ConfigureAwait(true);
    }

    private Task BuildTreeViewAsync(XmlTypeCodeConfiguration configuration)
    {
        TreeViewItems = new List<TreeViewItem>();
        return BuildTypeCodeConfigurationAsync(configuration);
    }

    private Task BuildTypeCodeConfigurationAsync(XmlTypeCodeConfiguration configuration)
    {
        var typeCodeConfigurationItem = new TreeViewItem
        {
            Header = $"TypeCodeConfiguration CloseCmd={configuration.CloseCmd}",
            IsExpanded = true
        };

        BuildAssemblyRoots(configuration, typeCodeConfigurationItem);

        TreeViewItems?.Add(typeCodeConfigurationItem);

        return Task.CompletedTask;
    }

    private void BuildAssemblyRoots(XmlTypeCodeConfiguration typeCodeConfiguration, TreeViewItem typeCodeConfigurationItem)
    {
        typeCodeConfigurationItem.MouseRightButtonDown += async (_, _) =>
        {
            var result = await _wizardNavigationService.OpenWizardAsync(new WizardParameter<AssemblyRootWizardViewModel>
            {
                FinishButtonText = "Add"
            }, new NavigationContext()).ConfigureAwait(true);

            typeCodeConfigurationItem.Items.Add(new TreeViewItem
            {
                Header = $"AssemlbyRoot Path={result.Path} Priority={result.Priority}"
            });

            typeCodeConfiguration.AssemblyRoot.Add(new XmlAssemblyRoot
            {
                Path = result.Path ?? throw new Exception(),
                Priority = result.Priority ?? throw new Exception()
            });
        };
        
        foreach (var assemblyRoot in typeCodeConfiguration.AssemblyRoot)
        {
            var assemblyRootItem = new TreeViewItem
            {
                Header = $"AssemlbyRoot Path={assemblyRoot.Path} Priority={assemblyRoot.Priority}",
                IsExpanded = true
            };

            BuildIncludeAssemblyPatterns(assemblyRoot, assemblyRootItem);

            BuildAssemblyGroups(assemblyRoot, assemblyRootItem);

            typeCodeConfigurationItem.Items.Add(assemblyRootItem);
        }
    }

    private static void BuildAssemblyGroups(XmlAssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var assemblyGroup in assemblyRoot.AssemblyGroup)
        {
            var assemblyGroupItem = new TreeViewItem
            {
                Header = $"AssemlbyGroup Name={assemblyGroup.Name} Priority={assemblyGroup.Priority}",
                IsExpanded = true
            };

            BuildAssemblyPaths(assemblyGroup, assemblyGroupItem);

            assemblyRootItem.Items.Add(assemblyGroupItem);
        }
    }
    
    private static void BuildIncludeAssemblyPatterns(XmlAssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var includeAssemblyPattern in assemblyRoot.IncludeAssemblyPattern)
        {
            var includeAssemblyPatternItem = new TreeViewItem
            {
                Header = $"IncludeAssemblyPattern Value={includeAssemblyPattern}",
                IsExpanded = true
            };

            assemblyRootItem.Items.Add(includeAssemblyPatternItem);
        }
    }

    private static void BuildAssemblyPaths(XmlAssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPath in assemblyGroup.AssemblyPath)
        {
            var assemblyPathItem = new TreeViewItem
            {
                Header = $"AssemblyPath Priority={assemblyPath.Priority} Value={assemblyPath.Text}",
                IsExpanded = true
            };

            assemblyGroupItem.Items.Add(assemblyPathItem);
        }
    }

    public List<TreeViewItem>? TreeViewItems
    {
        get => Get<List<TreeViewItem>?>();
        set => Set(value);
    }

    private async Task<XmlTypeCodeConfiguration> ReadXmlConfigurationAsync()
    {
        var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }

    private Task WriteXmlConfigurationAsync(XmlTypeCodeConfiguration xmlConfiguration)
    {
        var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var serialized = _genericXmlSerializer.Serialize(xmlConfiguration);
        return File.WriteAllTextAsync(cfg, serialized);
    }
}