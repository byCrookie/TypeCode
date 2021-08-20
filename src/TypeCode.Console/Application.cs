using System.Threading;
using System.Threading.Tasks;
using Framework.Boot;
using TypeCode.Business;

namespace TypeCode.Console
{
    public class Application : IApplication
    {
        private readonly ITypeCode _typeCode;

        public Application(ITypeCode typeCode)
        {
            _typeCode = typeCode;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            return _typeCode.StartAsync();
        }
    }
}