namespace TypeCode.Business.Configuration.Assemblies;

public interface IAssemblyLoader
{
    public Task LoadAsync(TypeCodeConfiguration configuration);
}