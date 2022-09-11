using Jab;

namespace TypeCode.Wpf.Pages.Composer;

[ServiceProviderModule]
[Singleton(typeof(ComposerView))]
[Singleton(typeof(ComposerViewModel))]
public interface IComposerModule
{
    
}