using System.Threading.Tasks;

namespace TypeCode.Console.Mode
{
	internal interface ITypeCodeStrategy
	{
		int Number();
		string Description();
		bool IsPlanned();
		bool IsBeta();
		bool IsResponsibleFor(string mode);
		Task<string> GenerateAsync();
	}
}