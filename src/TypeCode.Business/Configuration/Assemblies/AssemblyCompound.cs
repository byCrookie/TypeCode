namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyCompound
{
    public AssemblyCompound(string file)
    {
        File = file;
        Types = new List<Type>();
        LastFileWriteTime = System.IO.File.GetLastWriteTime(file);
    }
    
    public string File { get; }
    public System.Reflection.Assembly? Assembly { get; set; }
    public List<Type> Types { get; set; }
    public DateTime LastFileWriteTime { get; set; }
}