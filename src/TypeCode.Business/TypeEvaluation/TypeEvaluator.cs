using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Util;
using Framework.Extensions.List;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation
{
    internal class TypeEvaluator : ITypeEvaluator
    {
        public TypeCodeConfiguration EvaluateTypes(TypeCodeConfiguration configuration)
        {
            try
            {
                return LoadTypes(configuration);
            }
            catch (Exception ex)
            {
                throw new TypeLoadException("Loading types failed. " + ex.Message);
            }
        }

        private static TypeCodeConfiguration LoadTypes(TypeCodeConfiguration configuration)
        {
            Parallel.ForEach(configuration.AssemblyRoot, root =>
            {
                Parallel.ForEach(root.AssemblyGroup, group =>
                {
                    Parallel.ForEach(group.AssemblyPath, path =>
                    {
                        Parallel.ForEach(path.AssemblyDirectories, assemblyDirectory => { Parallel.ForEach(assemblyDirectory.Assemblies, assembly => Load(assembly, assemblyDirectory)); });
                    });

                    Parallel.ForEach(group.AssemblyPathSelector, selector =>
                    {
                        Parallel.ForEach(selector.AssemblyDirectories, assemblyDirectory => { Parallel.ForEach(assemblyDirectory.Assemblies, assembly => Load(assembly, assemblyDirectory)); });
                    });
                });
            });

            return configuration;
        }

        private static void Load(Assembly assembly, AssemblyDirectory assemblyDirectory)
        {
            var loadedTypes = assembly.GetLoadableTypes();
            loadedTypes.ForEach(type => assemblyDirectory.Types.Add(type));
        }
    }
}