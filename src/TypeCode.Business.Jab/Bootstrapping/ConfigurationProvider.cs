using System.Collections.Generic;
using System.Reflection;
using TypeCode.Business.Configuration;

namespace TypeCode.Business.Bootstrapping;

internal class ConfigurationProvider : IConfigurationProvider
{
	private static TypeCodeConfiguration _configuration;

	public void SetConfiguration(TypeCodeConfiguration configuration)
	{
		_configuration = configuration;
	}

	public TypeCodeConfiguration GetConfiguration()
	{
		return _configuration;
	}

	public IEnumerable<Assembly> GetAssemblies()
	{
		yield break;
	}
}