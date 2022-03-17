namespace TypeCode.Business.Configuration;

internal class AssemblyLoadException : Exception
{
	public AssemblyLoadException(string message) : base(message)
	{
	}
}