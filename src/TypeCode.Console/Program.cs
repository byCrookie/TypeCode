using System;
using System.Reflection;
using System.Threading.Tasks;
using Serilog;
using TypeCode.Business.Logging;
using TypeCode.Console.Boot;

namespace TypeCode.Console
{
	internal class Program
	{
		private static async Task<int> Main()
		{
			Log.Logger = LoggerConfigurationProvider.Create().CreateLogger();
			
			try
			{
				Log.Debug("Boot");
				await Bootstrapper.BootAsync().ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				WriteExceptionToLog(exception);
				return 1;
			}

			return 0;
		}

		private static void WriteExceptionToLog(Exception exception)
		{
			Log.Error(exception, "{0}", exception.Message);

			if (exception is ReflectionTypeLoadException reflectionTypeLoadException)
			{
				foreach (var loaderException in reflectionTypeLoadException.LoaderExceptions)
				{
					Log.Error(loaderException, "{0}", loaderException?.Message);
				}
			}
		}
	}
}