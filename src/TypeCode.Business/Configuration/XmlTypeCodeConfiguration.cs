using System.Xml.Serialization;

namespace TypeCode.Business.Configuration;

[XmlRoot(ElementName = "AssemblyPathSelector")]
public sealed class XmlAssemblyPathSelector
{
    public XmlAssemblyPathSelector()
    {
        Selector = string.Empty;
        Text = string.Empty;
    }
    
    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }

    [XmlAttribute(AttributeName = "Selector")]
    public string Selector { get; set; }

    [XmlText]
    public string Text { get; set; }
    
    [XmlAttribute(AttributeName = "Ignore")]
    public bool Ignore { get; set; }
}

[XmlRoot(ElementName = "AssemblyPath")]
public sealed class XmlAssemblyPath
{
    public XmlAssemblyPath()
    {
        Text = string.Empty;
    }
    
    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }

    [XmlText]
    public string Text { get; set; }
    
    [XmlAttribute(AttributeName = "Ignore")]
    public bool Ignore { get; set; }
}

[XmlRoot(ElementName = "AssemblyGroup")]
public sealed class XmlAssemblyGroup
{
    public XmlAssemblyGroup()
    {
        AssemblyPathSelector = new List<XmlAssemblyPathSelector>();
        AssemblyPath = new List<XmlAssemblyPath>();
        Name = string.Empty;
    }
        
    [XmlElement(ElementName = "AssemblyPathSelector")]
    public List<XmlAssemblyPathSelector> AssemblyPathSelector { get; set; }

    [XmlElement(ElementName = "AssemblyPath")]
    public List<XmlAssemblyPath> AssemblyPath { get; set; }

    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }
    
    [XmlAttribute(AttributeName = "Ignore")]
    public bool Ignore { get; set; }
}

[XmlRoot(ElementName = "AssemblyRoot")]
public sealed class XmlAssemblyRoot
{
    public XmlAssemblyRoot()
    {
        IncludeAssemblyPattern = new List<string>();
        AssemblyGroup = new List<XmlAssemblyGroup>();
        Path = string.Empty;
    }
        
    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }
        
    [XmlElement(ElementName = "IncludeAssemblyPattern")]
    public List<string> IncludeAssemblyPattern { get; set; }

    [XmlElement(ElementName = "AssemblyGroup")]
    public List<XmlAssemblyGroup> AssemblyGroup { get; set; }

    [XmlAttribute(AttributeName = "Path")]
    public string Path { get; set; }
    
    [XmlAttribute(AttributeName = "Ignore")]
    public bool Ignore { get; set; }
}

[XmlRoot(ElementName = "TypeCodeConfiguration")]
public sealed class XmlTypeCodeConfiguration
{
    public XmlTypeCodeConfiguration()
    {
        AssemblyRoot = new List<XmlAssemblyRoot>();
    }
        
    [XmlElement(ElementName = "CloseCmd")]
    public bool CloseCmd { get; set; }

    [XmlElement(ElementName = "AssemblyRoot")]
    public List<XmlAssemblyRoot> AssemblyRoot { get; set; }
}