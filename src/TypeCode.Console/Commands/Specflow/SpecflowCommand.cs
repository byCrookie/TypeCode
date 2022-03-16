﻿using System.Diagnostics.CodeAnalysis;
using Autofac;
using Cocona;
using JetBrains.Annotations;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Boot.Helper;

namespace TypeCode.Console.Commands.Specflow;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class SpecflowCommand
{
    [UsedImplicitly]
    public async Task ExecuteAsync(
        [Option] bool onlyRequired,
        [Option] bool sortAlphabetically,
        [Argument] string[] typeNames,
        [Option] bool includeStrings = true
    )
    {
        await using (var scope = LifeTimeScopeCreator.BeginLifetimeScope(ContextProvider.Get()))
        {
            var mode = scope.Resolve<ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter>>();
            var typeProvider = scope.Resolve<ITypeProvider>();
            var types = typeProvider.TryGetByNames(typeNames);
            var parameter = new SpecflowTypeCodeGeneratorParameter
            {
                Types = types.ToList(),
                OnlyRequired = onlyRequired,
                SortAlphabetically = sortAlphabetically,
                IncludeStrings = includeStrings
            };
            System.Console.WriteLine(await mode.GenerateAsync(parameter).ConfigureAwait(false));
        }
    }
}