using System.Reflection;
using Serilog;

namespace TypeCode.Business.TypeEvaluation;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
    {
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));
        
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            Log.Warning("Not all types have been loaded from {Assembly}", assembly.FullName);
            return e.Types.Where(type => type is not null).Cast<Type>();
        }
    }
}