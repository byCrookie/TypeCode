namespace TypeCode.Console.Interactive.Mode;

internal interface IModeComposer
{
	IEnumerable<ITypeCodeStrategy> ComposeOrdered();
}