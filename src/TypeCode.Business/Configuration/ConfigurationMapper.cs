namespace TypeCode.Business.Configuration;

public class ConfigurationMapper : IConfigurationMapper
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
        return xmlAssemblyRoots
            .Select(root =>
            {
                if (string.IsNullOrEmpty(root.Path)) throw new ArgumentException($"{nameof(root.Path)} can not be null or empty");

                return root;
            })
            .Select(root => new AssemblyRoot
        {
            Priority = root.Priority,
            Path = root.Path,
            AssemblyGroup = MapToConfiguration(root.AssemblyGroup).ToList(),
            IncludeAssemblyPattern = root.IncludeAssemblyPattern
        });
    }

    private static IEnumerable<AssemblyGroup> MapToConfiguration(
        IEnumerable<XmlAssemblyGroup> xmlConfigurationAssemblyGroups)
    {
        return xmlConfigurationAssemblyGroups
            .Select(xmlConfigurationAssemblyGroup =>
            {
                if (string.IsNullOrEmpty(xmlConfigurationAssemblyGroup.Name)) throw new ArgumentException($"{nameof(xmlConfigurationAssemblyGroup.Name)} can not be null or empty");

                return xmlConfigurationAssemblyGroup;
            })
            .Select(xmlConfigurationAssemblyGroup => new AssemblyGroup
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
        return xmlAssemblyPathSelectors
            .Select(xmlAssemblyPathSelector =>
            {
                if (string.IsNullOrEmpty(xmlAssemblyPathSelector.Text)) throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Text)} can not be null or empty");
                if (string.IsNullOrEmpty(xmlAssemblyPathSelector.Selector)) throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Selector)} can not be null or empty");

                return xmlAssemblyPathSelector;
            })
            .Select(xmlAssemblyPathSelector => new AssemblyPathSelector
            {
                Path = xmlAssemblyPathSelector.Text,
                Priority = xmlAssemblyPathSelector.Priority,
                Selector = xmlAssemblyPathSelector.Selector
            });
    }

    private static IEnumerable<AssemblyPath> MapToConfiguration(IEnumerable<XmlAssemblyPath> xmlAssemblyPaths)
    {
        return xmlAssemblyPaths.Select(xmlAssemblyPath =>
        {
            if (string.IsNullOrEmpty(xmlAssemblyPath.Text)) throw new ArgumentException($"{nameof(xmlAssemblyPath.Text)} can not be null or empty");
           
            return xmlAssemblyPath;
        }).Select(xmlAssemblyPath => new AssemblyPath
        {
            Path = xmlAssemblyPath.Text,
            Priority = xmlAssemblyPath.Priority
        });
    }
}