using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public interface IConfigurationTypeProvider : ITypeProvider
{
    Task InitalizeAsync(TypeCodeConfiguration configuration);
}