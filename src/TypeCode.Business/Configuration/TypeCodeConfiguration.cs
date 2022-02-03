using System.Collections.Concurrent;
using TypeCode.Business.Bootstrapping;

namespace TypeCode.Business.Configuration;

public class AssemblyGroup
{
    public AssemblyGroup()
    {
        AssemblyPathSelector = new List<AssemblyPathSelector>();
        AssemblyPath = new List<AssemblyPath>();
        PriorityAssemblyList = new List<PriorityString>();
    }
        
    public List<AssemblyPathSelector> AssemblyPathSelector { get; set; }
    public List<AssemblyPath> AssemblyPath { get; set; }
    public string? Name { get; set; }
    public int Priority { get; set; }
    public string? Text { get; set; }
    public List<PriorityString> PriorityAssemblyList { get; set; }
}
    
public class AssemblyRoot
{
    public AssemblyRoot()
    {
        IncludeAssemblyPattern = new List<string>();
        AssemblyGroup = new List<AssemblyGroup>();
    }
        
    public int Priority { get; set; }
    public List<string> IncludeAssemblyPattern { get; set; }
    public List<AssemblyGroup> AssemblyGroup { get; set; }
    public string? Path { get; set; }
    public string? Text { get; set; }
}
    
public class TypeCodeConfiguration
{
    public TypeCodeConfiguration()
    {
        AssemblyRoot = new List<AssemblyRoot>();
    }
        
    public List<AssemblyRoot> AssemblyRoot { get; set; }
}

public class AssemblyPath : IAssemblyHolder, IDictionaryHolder
{
    public AssemblyPath()
    {
        AssemblyDirectories = new ConcurrentBag<AssemblyDirectory>();
        TypesByNameDictionary = new Dictionary<string, List<Type>>();
        TypesByFullNameDictionary = new Dictionary<string, List<Type>>();   
    }
        
    public string? Path { get; set; }
    public int Priority { get; set; }
    public ConcurrentBag<AssemblyDirectory> AssemblyDirectories { get; set; }
    public IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
    public IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
}

public class AssemblyPathSelector : IAssemblyHolder, IDictionaryHolder
{
    public AssemblyPathSelector()
    {
        AssemblyDirectories = new ConcurrentBag<AssemblyDirectory>();
        TypesByNameDictionary = new Dictionary<string, List<Type>>();
        TypesByFullNameDictionary = new Dictionary<string, List<Type>>();
    }
        
    public string? Path { get; set; }
    public int Priority { get; set; }
    public string? Selector { get; set; }
    public ConcurrentBag<AssemblyDirectory> AssemblyDirectories { get; set; }
    public IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
    public IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
}