using System;
using System.Collections.Generic;
using Framework.Workflow;
using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;

namespace TypeCode.Console.Mode.UnitTestDependency
{
    internal class UnitTestDependencyEvaluationContext : WorkflowBaseContext, ISelectionContext, IMultipleTypesSelectionContext, IExitOrContinueContext
    {
        public string Input { get; set; }
        public string UnitTestDependencyCode { get; set; }
        public short Selection { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
    }
}