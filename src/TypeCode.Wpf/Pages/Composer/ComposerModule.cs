using Jab;

namespace TypeCode.Wpf.Pages.Composer;

[ServiceProviderModule]
[Transient(typeof(ComposerView))]
[Transient(typeof(ComposerViewModel))]
public partial interface IComposerModule
{
}