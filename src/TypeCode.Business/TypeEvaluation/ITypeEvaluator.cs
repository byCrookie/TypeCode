using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation
{
	internal interface ITypeEvaluator
	{
		TypeCodeConfiguration EvaluateTypes(TypeCodeConfiguration assemblies);
	}
}