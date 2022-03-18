using Jab;

namespace TypeCode.Wpf.Pages.Composer;

[ServiceProviderModule]
[Transient(typeof(ComposerView))]
[Transient(typeof(ComposerViewModel))]
public interface IComposerModule
{
    
}