namespace TypeCode.Wpf.Pages.TypeSelection;

public interface ITypeSelectionWizardStarter
{
    Task StartAsync(TypeSelectionParameter parameter, Func<TypeSelectionViewModel, Task> onSaveAction);
}