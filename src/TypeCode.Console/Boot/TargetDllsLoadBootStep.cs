﻿using Framework.Boot;
using TypeCode.Business.TypeEvaluation;
using Workflow;

namespace TypeCode.Console.Boot;

public sealed class TargetDllsLoadBootStep<TContext, TOptions> : ITargetDllsLoadBootStep<TContext, TOptions>
    where TContext : WorkflowBaseContext, IBootContext
{
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;
    
    private Lazy<TargetDllsBootStepOptions>? _options;

    public TargetDllsLoadBootStep(ILazyTypeProviderFactory lazyTypeProviderFactory)
    {
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
    }

    public Task ExecuteAsync(TContext context)
    {
        var options = _options!.Value;
        _lazyTypeProviderFactory.InitializeByDlls(options.DllPaths, options.DllDeep, options.DllPattern);
        return Task.CompletedTask;
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }

    public void SetOptions(Lazy<TOptions> options) => _options = options as Lazy<TargetDllsBootStepOptions>;
}