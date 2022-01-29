using System.Collections.Generic;

namespace TypeCode.Console.Mode;

internal interface IModeComposer
{
	IEnumerable<ITypeCodeStrategy> ComposeOrdered();
}