namespace TypeCode.Wpf.Pages.TypeSelection;

public interface ITypeSelectionWizardStarter
{
    Task StartAsync(TypeSelectionParameter parameter, Func<IEnumerable<Type>, Task> onSaveAction, Func<IEnumerable<Type>, Task> onCancelAction);
}