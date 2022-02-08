using Autofac;
using Framework.Autofac.Boot;
using Serilog;

namespace TypeCode.Console.Boot.Helper;

public static class LifeTimeScopeCreator
{
    public static ILifetimeScope BeginLifetimeScope(BootContext context)
    {
        Log.Debug("Begin Autofac LifeTimeScope");
        return context.Container.BeginLifetimeScope(builder =>
        {
            foreach (var registrationAction in context.RegistrationActions)
                registrationAction(builder);
        });
    }
}