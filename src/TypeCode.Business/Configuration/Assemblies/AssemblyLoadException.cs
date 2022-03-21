namespace TypeCode.Business.Configuration.Assemblies;

internal class AssemblyLoadException : Exception
{
	public AssemblyLoadException(string message) : base(message)
	{
	}
}