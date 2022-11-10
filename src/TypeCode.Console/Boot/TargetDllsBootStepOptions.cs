namespace TypeCode.Console.Boot;

public class TargetDllsBootStepOptions
{
    public TargetDllsBootStepOptions()
    {
        DllPaths = new List<string>();
        DllPattern = "*.dll";
        DllDeep = false;
    }
    
    public IEnumerable<string> DllPaths { get; set; }
    public string DllPattern { get; set; }
    public bool DllDeep { get; set; }
}