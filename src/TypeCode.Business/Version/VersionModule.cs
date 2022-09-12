using DependencyInjection.Microsoft.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace TypeCode.Business.Version;

public sealed class VersionModule : Module
{
    public override void Load(IServiceCollection services)
    {
        services.AddTransient<IVersionEvaluator, VersionEvaluator>();
        
        base.Load(services);
    }
}