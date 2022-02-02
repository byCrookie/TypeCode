using System.Xml.Serialization;

namespace TypeCode.Business.Configuration;

[XmlRoot(ElementName = "AssemblyPathSelector")]
public class XmlAssemblyPathSelector
{
    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }

    [XmlAttribute(AttributeName = "Selector")]
    public string Selector { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "AssemblyPath")]
public class XmlAssemblyPath
{
    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "AssemblyGroup")]
public class XmlAssemblyGroup
{
    public XmlAssemblyGroup()
    {
        AssemblyPathSelector = new List<XmlAssemblyPathSelector>();
        AssemblyPath = new List<XmlAssemblyPath>();
    }
        
    [XmlElement(ElementName = "AssemblyPathSelector")]
    public List<XmlAssemblyPathSelector> AssemblyPathSelector { get; set; }

    [XmlElement(ElementName = "AssemblyPath")]
    public List<XmlAssemblyPath> AssemblyPath { get; set; }

    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "AssemblyRoot")]
public class XmlAssemblyRoot
{
    public XmlAssemblyRoot()
    {
        IncludeAssemblyPattern = new List<string>();
        AssemblyGroup = new List<XmlAssemblyGroup>();
    }
        
    [XmlAttribute(AttributeName = "Priority")]
    public int Priority { get; set; }
        
    [XmlElement(ElementName = "IncludeAssemblyPattern")]
    public List<string> IncludeAssemblyPattern { get; set; }

    [XmlElement(ElementName = "AssemblyGroup")]
    public List<XmlAssemblyGroup> AssemblyGroup { get; set; }

    [XmlAttribute(AttributeName = "Path")]
    public string Path { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "TypeCodeConfiguration")]
public class XmlTypeCodeConfiguration
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