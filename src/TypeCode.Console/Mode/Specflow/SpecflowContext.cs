using System;
using System.Collections.Generic;
using Framework.Workflow;
using TypeCode.Console.Mode.ExitOrContinue;

namespace TypeCode.Console.Mode.Specflow
{
    internal class SpecflowContext : WorkflowBaseContext, IExitOrContinueContext
    {
        public string Input { get; set; }
        public string Tables { get; set; }
        public IEnumerable<Type> Types { get; set; }
    }
}