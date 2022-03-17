using System.Collections.Concurrent;
using System.Reflection;

namespace TypeCode.Business.Configuration;

public class AssemblyDirectory
{
    public AssemblyDirectory(string relativePath, string absolutPath, IEnumerable<string> files)
    {
        RelativePath = relativePath;
        AbsolutPath = absolutPath;
        Files = files;
        Assemblies = new ConcurrentBag<Assembly>();
        Types = new ConcurrentBag<Type>();
    }
        
    public string RelativePath { get; set; }
    public string AbsolutPath { get; set; }
    public IEnumerable<string> Files { get; set; }
    public ConcurrentBag<Assembly> Assemblies { get; set; }
    public ConcurrentBag<Type> Types { get; set; }
}