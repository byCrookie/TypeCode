using System.Reflection;

namespace TypeCode.Business.Configuration;

public interface IAssemblyFileLoader
{
    public Task<Assembly> LoadAsync(string path);
}