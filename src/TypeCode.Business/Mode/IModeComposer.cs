using System.Collections.Generic;

namespace TypeCode.Business.Mode
{
	internal interface IModeComposer
	{
		IEnumerable<ITypeCodeStrategy> ComposeOrdered();
	}
}