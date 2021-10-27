using System.Collections.Generic;
using Framework.Workflow;
using TypeCode.Console.Mode;

namespace TypeCode.Console
{
    internal class TypeCodeContext : WorkflowBaseContext
    {
        public string Input { get; set; }
        public ITypeCodeStrategy Mode { get; set; }
        public IEnumerable<ITypeCodeStrategy> Modes { get; set; }
    }
}