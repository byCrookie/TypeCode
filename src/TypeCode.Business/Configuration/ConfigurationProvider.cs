﻿namespace TypeCode.Business.Configuration;

public class ConfigurationProvider : IConfigurationProvider
{
	private static TypeCodeConfiguration? _configuration;

	public void SetConfiguration(TypeCodeConfiguration? configuration)
	{
		_configuration = configuration;
	}

	public TypeCodeConfiguration GetConfiguration()
	{
		if (_configuration is null)
		{
			throw new ArgumentNullException($"{nameof(TypeCodeConfiguration)} not yet set");
		}
		
		return _configuration;
	}
}