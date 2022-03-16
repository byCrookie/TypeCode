using System.Reflection;

namespace TypeCode.Business.Bootstrapping;

public interface IAssemblyLoader
{
    public Task<Assembly> LoadAsync(string path);
}