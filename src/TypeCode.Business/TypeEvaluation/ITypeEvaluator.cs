using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public interface ITypeEvaluator
{
	TypeCodeConfiguration EvaluateTypes(TypeCodeConfiguration assemblies);
}