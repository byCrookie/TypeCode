using System.Reflection;

namespace TypeCode.Business.Embedded;

public interface IResourceReader
{
    string ReadResource(Assembly assembly, string resourcePath);
}