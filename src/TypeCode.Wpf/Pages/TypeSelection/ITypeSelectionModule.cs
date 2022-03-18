using Jab;

namespace TypeCode.Wpf.Pages.TypeSelection;

[ServiceProviderModule]
[Transient(typeof(TypeSelectionView))]
[Transient(typeof(TypeSelectionViewModel))]
[Transient(typeof(ITypeSelectionWizardStarter), typeof(TypeSelectionWizardStarter))]
public interface ITypeSelectionModule
{
}