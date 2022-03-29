namespace TypeCode.Business.Configuration;

public class ConfigurationProvider : IConfigurationProvider
{
	private static TypeCodeConfiguration? _configuration;

	public void Set(TypeCodeConfiguration? configuration)
	{
		_configuration = configuration;
	}

	public TypeCodeConfiguration Get()
	{
		if (_configuration is null)
		{
			throw new ArgumentNullException($"{nameof(TypeCodeConfiguration)} not yet set");
		}
		
		return _configuration;
	}

	public bool IsSet()
	{
		return _configuration != null;
	}
}