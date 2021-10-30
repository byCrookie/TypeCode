using System;
using System.Collections.Generic;
using Workflow;

namespace TypeCode.Console.Mode.MultipleTypes
{
    internal class MultipleTypesSelectionContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
        public string Input { get; set; }
    }
}