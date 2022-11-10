namespace TypeCode.Console.Boot;

public class TargetDllsBootStepOptions
{
    public TargetDllsBootStepOptions()
    {
        TargetDllPaths = new List<string>();
    }
    
    public IEnumerable<string> TargetDllPaths { get; set; }
}