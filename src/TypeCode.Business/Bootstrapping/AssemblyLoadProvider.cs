using TypeCode.Business.Configuration;

namespace TypeCode.Business.Bootstrapping
{
	public static class AssemblyLoadProvider
	{
		public static TypeCodeConfiguration GetConfiguration()
		{
			if (AssemblyProvider != null)
			{
				return AssemblyProvider.GetConfiguration();
			}

			throw new AssemblyLoadException("No AssemblyProvider was set.");
		}

		public static void SetAssemblyProvider(IConfigurationProvider assemblyProvider)
		{
			AssemblyProvider = assemblyProvider;
		}

		private static IConfigurationProvider AssemblyProvider { get; set; }
	}
}