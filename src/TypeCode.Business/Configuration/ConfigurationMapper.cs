using System.Text.RegularExpressions;

namespace TypeCode.Business.Configuration;

public class ConfigurationMapper : IConfigurationMapper
{
    public TypeCodeConfiguration MapToConfiguration(XmlTypeCodeConfiguration xmlConfiguration)
    {
        return new TypeCodeConfiguration
        {
            AssemblyRoot = MapToConfiguration(xmlConfiguration.AssemblyRoot).ToList(),
            CloseCmd = xmlConfiguration.CloseCmd
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
                Ignore = root.Ignore,
                AssemblyGroup = MapToConfiguration(root.AssemblyGroup).ToList(),
                IncludeAssemblyPattern = root.IncludeAssemblyPattern
                    .Select(pattern => new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    .ToList()
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
                Ignore = xmlConfigurationAssemblyGroup.Ignore,
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
                Selector = xmlAssemblyPathSelector.Selector,
                Ignore = xmlAssemblyPathSelector.Ignore
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
            Priority = xmlAssemblyPath.Priority,
            Ignore = xmlAssemblyPath.Ignore
        });
    }

    public XmlTypeCodeConfiguration MapToXml(TypeCodeConfiguration configuration)
    {
        return new XmlTypeCodeConfiguration
        {
            AssemblyRoot = MapToXml(configuration.AssemblyRoot).ToList(),
            CloseCmd = configuration.CloseCmd
        };
    }

    private static IEnumerable<XmlAssemblyRoot> MapToXml(IEnumerable<AssemblyRoot> assemblyRoots)
    {
        return assemblyRoots
            .Select(root =>
            {
                if (string.IsNullOrEmpty(root.Path)) throw new ArgumentException($"{nameof(root.Path)} can not be null or empty");
                return root;
            })
            .Select(root => new XmlAssemblyRoot
            {
                Priority = root.Priority,
                Path = root.Path,
                Ignore = root.Ignore,
                AssemblyGroup = MapToXml(root.AssemblyGroup).ToList(),
                IncludeAssemblyPattern = root.IncludeAssemblyPattern.Select(regex => regex.ToString()).ToList()
            });
    }

    private static IEnumerable<XmlAssemblyGroup> MapToXml(
        IEnumerable<AssemblyGroup> configurationAssemblyGroups)
    {
        return configurationAssemblyGroups
            .Select(xmlConfigurationAssemblyGroup =>
            {
                if (string.IsNullOrEmpty(xmlConfigurationAssemblyGroup.Name)) throw new ArgumentException($"{nameof(xmlConfigurationAssemblyGroup.Name)} can not be null or empty");

                return xmlConfigurationAssemblyGroup;
            })
            .Select(xmlConfigurationAssemblyGroup => new XmlAssemblyGroup
            {
                Name = xmlConfigurationAssemblyGroup.Name,
                Priority = xmlConfigurationAssemblyGroup.Priority,
                Ignore = xmlConfigurationAssemblyGroup.Ignore,
                AssemblyPath = MapToXml(xmlConfigurationAssemblyGroup.AssemblyPath).ToList(),
                AssemblyPathSelector = MapToXml(xmlConfigurationAssemblyGroup.AssemblyPathSelector).ToList()
            });
    }

    private static IEnumerable<XmlAssemblyPathSelector> MapToXml(
        IEnumerable<AssemblyPathSelector> assemblyPathSelectors)
    {
        return assemblyPathSelectors
            .Select(xmlAssemblyPathSelector =>
            {
                if (string.IsNullOrEmpty(xmlAssemblyPathSelector.Path)) throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Path)} can not be null or empty");
                if (string.IsNullOrEmpty(xmlAssemblyPathSelector.Selector)) throw new ArgumentException($"{nameof(xmlAssemblyPathSelector.Selector)} can not be null or empty");

                return xmlAssemblyPathSelector;
            })
            .Select(xmlAssemblyPathSelector => new XmlAssemblyPathSelector
            {
                Text = xmlAssemblyPathSelector.Path,
                Priority = xmlAssemblyPathSelector.Priority,
                Selector = xmlAssemblyPathSelector.Selector,
                Ignore = xmlAssemblyPathSelector.Ignore
            });
    }

    private static IEnumerable<XmlAssemblyPath> MapToXml(IEnumerable<AssemblyPath> assemblyPaths)
    {
        return assemblyPaths.Select(xmlAssemblyPath =>
        {
            if (string.IsNullOrEmpty(xmlAssemblyPath.Path)) throw new ArgumentException($"{nameof(xmlAssemblyPath.Path)} can not be null or empty");

            return xmlAssemblyPath;
        }).Select(xmlAssemblyPath => new XmlAssemblyPath
        {
            Text = xmlAssemblyPath.Path,
            Priority = xmlAssemblyPath.Priority,
            Ignore = xmlAssemblyPath.Ignore
        });
    }
}