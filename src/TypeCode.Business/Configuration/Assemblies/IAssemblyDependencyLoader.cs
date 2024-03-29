﻿using System.Reflection;

namespace TypeCode.Business.Configuration.Assemblies;

public interface IAssemblyDependencyLoader
{
    Task<Assembly?> LoadWithDependenciesAsync(AssemblyRootCompound assemblyDirectory, string assemblyFullPath);
}