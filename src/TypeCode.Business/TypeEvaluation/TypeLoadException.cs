using System;

namespace TypeCode.Business.TypeEvaluation;

internal class TypeLoadException : Exception
{
	public TypeLoadException(string message)
		: base(message)
	{
	}
}