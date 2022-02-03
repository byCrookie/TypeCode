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
            Priority = root.Priority ?? throw new ArgumentException($"{nameof(root.Priority)} can not be null"),
            Path = root.Path ?? throw new ArgumentException($"{nameof(root.Path)} can not be null"),
            Text = root.Text ?? throw new ArgumentException($"{nameof(root.Text)} can not be null"),
            AssemblyGroup = MapToConfiguration(root.AssemblyGroup).ToList(),
            IncludeAssemblyPattern = root.IncludeAssemblyPattern
        });
    }

    private static IEnumerable<AssemblyGroup> MapToConfiguration(
        IEnumerable<XmlAssemblyGroup> xmlConfigurationAssemblyGroups)
    {
        return xmlConfigurationAssemblyGroups.Select(xmlConfigurationAssemblyGroup => new AssemblyGroup
        {
            Name = xmlConfigurationAssemblyGroup.Name ?? throw new ArgumentException($"{nameof(xmlConfigurationAssemblyGroup.Name)} can not be null"),
            Priority = xmlConfigurationAssemblyGroup.Priority ?? throw new ArgumentException($"{nameof(xmlConfigurationAssemblyGroup.Priority)} can not be null"),
            AssemblyPath = MapToConfiguration(xmlConfigurationAssemblyGroup.AssemblyPath).ToList(),
            AssemblyPathSelector = MapToConfiguration(xmlConfigurationAssemblyGroup.AssemblyPathSelector).ToList()
        });
    }

    private static IEnumerable<AssemblyPathSelector> MapToConfiguration(
        IEnumerable<XmlAssemblyPathSelector> xmlAssemblyPathSelectors)
    {
        return xmlAssemblyPathSelectors.Select(xmlAssemblyPathSelector => new AssemblyPathSelector
        {
            Path = xmlAssemblyPathSelector.Text ?? throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Text)} can not be null"),
            Priority = xmlAssemblyPathSelector.Priority ?? throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Priority)} can not be null"),
            Selector = xmlAssemblyPathSelector.Selector ?? throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Selector)} can not be null")
        });
    }

    private static IEnumerable<AssemblyPath> MapToConfiguration(IEnumerable<XmlAssemblyPath> xmlAssemblyPaths)
    {
        return xmlAssemblyPaths.Select(xmlAssemblyPath => new AssemblyPath
        {
            Path = xmlAssemblyPath.Text ?? throw new ArgumentException($"{nameof(xmlAssemblyPath.Text)} can not be null"),
            Priority = xmlAssemblyPath.Priority ?? throw new ArgumentException($"{nameof(xmlAssemblyPath.Priority)} can not be null")
        });
    }
}