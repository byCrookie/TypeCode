using System.Reflection;

namespace TypeCode.Business.Embedded;

public class ResourceReader : IResourceReader
{
    public string ReadResource(Assembly assembly, string resourcePath)
    {
        using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourcePath}"))
        {
            using (var reader = new StreamReader(stream ?? Stream.Null))
            {
                return reader.ReadToEnd();
            }
        }
    }
}