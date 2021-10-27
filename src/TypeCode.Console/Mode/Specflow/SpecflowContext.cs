using System;
using System.Collections.Generic;
using Framework.Workflow;

namespace TypeCode.Console.Mode.Specflow
{
    internal class SpecflowContext : WorkflowBaseContext
    {
        public string Input { get; set; }
        public string Tables { get; set; }
        public IEnumerable<Type> Types { get; set; }
    }
}