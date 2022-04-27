﻿using System.IO;
using System.Runtime.CompilerServices;
using Serilog;

namespace TypeCode.Wpf.Pages.DynamicExecute.Code;

public class Runner : IRunner
{
    public string Execute(byte[] compiledAssembly)
    {
        var weakReference = LoadAndExecute(compiledAssembly);

        for (var i = 0; i < 8 && weakReference.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        if (weakReference.IsAlive)
        {
            Log.Error("Unloading failed");
        }

        return string.Empty;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference LoadAndExecute(byte[] compiledAssembly)
    {
        using (var asm = new MemoryStream(compiledAssembly))
        {
            var assemblyLoadContext = new UnloadableAssemblyLoadContext();

            var assembly = assemblyLoadContext.LoadFromStream(asm);

            var entry = assembly.EntryPoint;

            _ = entry != null && entry.GetParameters().Length > 0
                ? entry.Invoke(null, new object?[] { new[] { "Easter Egg" } })
                : entry?.Invoke(null, null);

            assemblyLoadContext.Unload();

            return new WeakReference(assemblyLoadContext);
        }
    }
}