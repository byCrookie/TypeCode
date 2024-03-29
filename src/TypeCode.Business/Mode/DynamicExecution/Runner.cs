﻿using System.Runtime.CompilerServices;
using Serilog;

namespace TypeCode.Business.Mode.DynamicExecution;

public sealed class Runner : IRunner
{
    private readonly ICompiler _compiler;

    public Runner(ICompiler compiler)
    {
        _compiler = compiler;
    }

    public string CompileAndExecute(string sourceCode, params string?[] parameters)
    {
        return Execute(_compiler.Compile(sourceCode), parameters);
    }

    public string Execute(byte[] compiledAssembly, params string?[] parameters)
    {
        var sw = new StringWriter();
        Console.SetOut(sw);
        Console.SetError(sw);

        var weakReference = LoadAndExecute(compiledAssembly, parameters);

        for (var i = 0; i < 8 && weakReference.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        if (weakReference.IsAlive)
        {
            Log.Error("Unloading failed");
        }

        return sw.ToString();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference LoadAndExecute(byte[] compiledAssembly, params string?[] parameters)
    {
        using (var asm = new MemoryStream(compiledAssembly))
        {
            var assemblyLoadContext = new UnloadableAssemblyLoadContext();

            var assembly = assemblyLoadContext.LoadFromStream(asm);

            var entry = assembly.EntryPoint;

            _ = entry != null && entry.GetParameters().Length > 0
                ? entry.Invoke(null,  new object?[] { parameters.Any() ? parameters.ToArray() : Array.Empty<string?>() })
                : entry?.Invoke(null, null);

            assemblyLoadContext.Unload();

            return new WeakReference(assemblyLoadContext);
        }
    }
}