using System;
using System.Collections.Generic;
using Framework.Workflow;
using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.MultipleTypes;

namespace TypeCode.Console.Mode.Composer
{
    internal class ComposerContext : WorkflowBaseContext, IMultipleTypesSelectionContext, IExitOrContinueContext
    {
        public string TypeName { get; set; }
        public string ComposerCode { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
    }
}