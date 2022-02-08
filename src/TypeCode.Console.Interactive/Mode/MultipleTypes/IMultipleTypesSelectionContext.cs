namespace TypeCode.Console.Interactive.Mode.MultipleTypes;

internal interface IMultipleTypesSelectionContext
{
    List<Type> SelectedTypes { get; set; }
    Type? SelectedType { get; set; }
}