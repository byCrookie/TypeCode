namespace TypeCode.Business.Configuration;

public class AssemblyDirectory
{
    public AssemblyDirectory(string relativePath, string absolutPath)
    {
        RelativePath = relativePath;
        AbsolutPath = absolutPath;
        AssemblyCompounds = new List<AssemblyCompound>();
    }
        
    public string RelativePath { get; set; }
    public string AbsolutPath { get; set; }
    public CustomAssemblyLoadContext? AssemblyLoadContext { get; set; }
    public List<AssemblyCompound> AssemblyCompounds { get; set; }
}