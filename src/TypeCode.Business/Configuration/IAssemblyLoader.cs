namespace TypeCode.Business.Configuration;

public interface IAssemblyLoader
{
    public Task LoadAsync(TypeCodeConfiguration configuration);
}