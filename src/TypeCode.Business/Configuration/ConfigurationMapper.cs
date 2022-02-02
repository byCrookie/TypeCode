namespace TypeCode.Business.Configuration;

internal class ConfigurationMapper : IConfigurationMapper
{
    public TypeCodeConfiguration MapToConfiguration(XmlTypeCodeConfiguration xmlConfiguration)
    {
        return new TypeCodeConfiguration
        {
            AssemblyRoot = MapToConfiguration(xmlConfiguration.AssemblyRoot).ToList()
        };
    }

    private static IEnumerable<AssemblyRoot> MapToConfiguration(IEnumerable<XmlAssemblyRoot> xmlAssemblyRoots)
    {
        return xmlAssemblyRoots.Select(root => new AssemblyRoot
        {
            Priority = root.Priority,
            Path = root.Path,
            Text = root.Text,
            AssemblyGroup = MapToConfiguration(root.AssemblyGroup).ToList(),
            IncludeAssemblyPattern = root.IncludeAssemblyPattern
        });
    }

    private static IEnumerable<AssemblyGroup> MapToConfiguration(
        IEnumerable<XmlAssemblyGroup> xmlConfigurationAssemblyGroups)
    {
        return xmlConfigurationAssemblyGroups.Select(xmlConfigurationAssemblyGroup => new AssemblyGroup
        {
            Name = xmlConfigurationAssemblyGroup.Name,
            Priority = xmlConfigurationAssemblyGroup.Priority,
            AssemblyPath = MapToConfiguration(xmlConfigurationAssemblyGroup.AssemblyPath).ToList(),
            AssemblyPathSelector = MapToConfiguration(xmlConfigurationAssemblyGroup.AssemblyPathSelector).ToList()
        });
    }

    private static IEnumerable<AssemblyPathSelector> MapToConfiguration(
        IEnumerable<XmlAssemblyPathSelector> xmlAssemblyPathSelectors)
    {
        return xmlAssemblyPathSelectors.Select(xmlAssemblyPathSelector => new AssemblyPathSelector
        {
            Path = xmlAssemblyPathSelector.Text,
            Priority = xmlAssemblyPathSelector.Priority,
            Selector = xmlAssemblyPathSelector.Selector
        });
    }

    private static IEnumerable<AssemblyPath> MapToConfiguration(IEnumerable<XmlAssemblyPath> xmlAssemblyPaths)
    {
        return xmlAssemblyPaths.Select(xmlAssemblyPath => new AssemblyPath
        {
            Path = xmlAssemblyPath.Text,
            Priority = xmlAssemblyPath.Priority
        });
    }
}