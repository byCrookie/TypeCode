using System;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using TypeCode.Console.Boot;

namespace TypeCode.Console
{
	internal class Program
	{
		private static ILog _logger;

		private static async Task<int> Main()
		{
			var exitcode = 0;
			
			try
			{
				_logger = LogManager.GetLogger(typeof(Program));
				await TypeCodeBootstrapper.BootAsync().ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				exitcode = 1;
				WriteExceptionToLog(exception);
				System.Console.WriteLine(exception.Message);
			}

			return exitcode;
		}

		private static void WriteExceptionToLog(Exception exception)
		{
			_logger.Error(exception.Message, exception);

			if (exception is ReflectionTypeLoadException reflectionTypeLoadException)
			{
				foreach (var loaderException in reflectionTypeLoadException.LoaderExceptions)
				{
					_logger.Error(loaderException?.Message, loaderException);
				}
			}
		}
	}
}