using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace TypeCode.Business.Bootstrapping;

public class AssemblyDirectory
{
    public AssemblyDirectory()
    {
        Files = new List<string>();
        Assemblies = new ConcurrentBag<Assembly>();
        Types = new ConcurrentBag<Type>();
    }
        
    public string RelativePath { get; set; }
    public string AbsolutPath { get; set; }
    public IEnumerable<string> Files { get; set; }
    public ConcurrentBag<Assembly> Assemblies { get; set; }
    public ConcurrentBag<Type> Types { get; set; }
}