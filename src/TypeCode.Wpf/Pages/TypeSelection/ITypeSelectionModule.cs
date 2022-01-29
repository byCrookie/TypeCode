using Jab;

namespace TypeCode.Wpf.Pages.TypeSelection;

[ServiceProviderModule]
[Transient(typeof(TypeSelectionView))]
[Transient(typeof(TypeSelectionViewModel))]
public partial interface ITypeSelectionModule
{
}