using System;

namespace TypeCode.Business.Bootstrapping
{
	internal class AssemblyLoadException : Exception
	{
		public AssemblyLoadException(string message) : base(message)
		{
		}
	}
}