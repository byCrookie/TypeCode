using Framework.Autofac.Boot;

namespace TypeCode.Console;

public interface ITypeCode<in TContext> : IApplication<TContext> where TContext : BootContext
{
}