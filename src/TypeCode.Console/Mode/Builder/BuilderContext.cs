using System;
using System.Collections.Generic;
using Framework.Workflow;
using TypeCode.Console.Mode.MultipleTypes;

namespace TypeCode.Console.Mode.Builder
{
    internal class BuilderContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {
        public string TypeName { get; set; }
        public string BuilderCode { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
    }
}