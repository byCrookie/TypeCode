using System;
using System.Collections.Generic;
using Framework.Workflow;
using TypeCode.Business.Mode.MultipleTypes;

namespace TypeCode.Business.Mode.Composer
{
    internal class ComposerContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {
        public string TypeName { get; set; }
        public string ComposerCode { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
    }
}