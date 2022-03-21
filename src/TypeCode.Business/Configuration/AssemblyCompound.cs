using System.Reflection;

namespace TypeCode.Business.Configuration;

public class AssemblyCompound
{
    public AssemblyCompound(string file)
    {
        File = file;
        Types = new List<Type>();
        LastFileWriteTime = System.IO.File.GetLastWriteTime(file);
    }
    
    public string File { get; }
    public Assembly? Assembly { get; set; }
    public List<Type> Types { get; set; }
    public DateTime LastFileWriteTime { get; set; }
}