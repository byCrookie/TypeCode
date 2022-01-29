using System.Threading.Tasks;

namespace TypeCode.Business.Mode
{
	public interface ITypeCodeGenerator<in T> where T : ITypeCodeGeneratorParameter
	{
		Task<string> GenerateAsync(T parameter);
	}
}