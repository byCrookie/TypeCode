using System;
using System.Collections.Generic;
using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.MultipleTypes;
using Workflow;

namespace TypeCode.Console.Mode.Composer
{
    internal class ComposerContext : WorkflowBaseContext, IMultipleTypesSelectionContext, IExitOrContinueContext
    {
        public string ComposerCode { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
        public string Input { get; set; }
    }
}