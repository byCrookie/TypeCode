using System.Reflection;
using System.Runtime.Loader;

namespace TypeCode.Business.Configuration;

public class AssemblyCompound
{
    public AssemblyCompound(string file)
    {
        File = file;
        Types = new List<Type>();
    }
    
    public string File { get; }
    public Assembly? Assembly { get; set; }
    public AssemblyLoadContext? AssemblyLoadContext { get; set; }
    public List<Type> Types { get; set; }
}