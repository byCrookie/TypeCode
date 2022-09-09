using System.Text.RegularExpressions;
using TypeCode.Business.Configuration.Assemblies;

namespace TypeCode.Business.Configuration;

public class AssemblyGroup
{
    public AssemblyGroup()
    {
        AssemblyPathSelector = new List<AssemblyPathSelector>();
        AssemblyPath = new List<AssemblyPath>();
        PriorityAssemblyList = new List<AssemblyPriorityString>();
        Name = string.Empty;
    }
        
    public List<AssemblyPathSelector> AssemblyPathSelector { get; set; }
    public List<AssemblyPath> AssemblyPath { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }
    public bool Ignore { get; set; }
    public List<AssemblyPriorityString> PriorityAssemblyList { get; set; }
}
    
public class AssemblyRoot
{
    public AssemblyRoot()
    {
        IncludeAssemblyPattern = new List<Regex>();
        AssemblyGroup = new List<AssemblyGroup>();
        Path = string.Empty;
    }
        
    public int Priority { get; set; }
    public List<Regex> IncludeAssemblyPattern { get; set; }
    public List<AssemblyGroup> AssemblyGroup { get; set; }
    public string Path { get; set; }
    public bool Ignore { get; set; }
}
    
public class TypeCodeConfiguration
{
    public TypeCodeConfiguration()
    {
        AssemblyRoot = new List<AssemblyRoot>();
    }
    
    public bool CloseCmd { get; set; }
    public List<AssemblyRoot> AssemblyRoot { get; set; }
}

public class AssemblyPath : IAssemblyHolder, IDictionaryHolder
{
    public AssemblyPath()
    {
        AssemblyDirectories = new List<AssemblyDirectory>();
        TypesByNameDictionary = new Dictionary<string, List<Type>>();
        TypesByFullNameDictionary = new Dictionary<string, List<Type>>();
        Path = string.Empty;
    }
        
    public string Path { get; set; }
    public int Priority { get; set; }
    public bool Ignore { get; set; }
    public List<AssemblyDirectory> AssemblyDirectories { get; set; }
    public IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
    public IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
}

public class AssemblyPathSelector : IAssemblyHolder, IDictionaryHolder
{
    public AssemblyPathSelector()
    {
        AssemblyDirectories = new List<AssemblyDirectory>();
        TypesByNameDictionary = new Dictionary<string, List<Type>>();
        TypesByFullNameDictionary = new Dictionary<string, List<Type>>();
        Path = string.Empty;
        Selector = string.Empty;
    }
        
    public string Path { get; set; }
    public int Priority { get; set; }
    public string Selector { get; set; }
    public bool Ignore { get; set; }
    public List<AssemblyDirectory> AssemblyDirectories { get; set; }
    public IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
    public IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
}