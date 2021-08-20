using System.Collections.Generic;
using System.Linq;
using Framework.Workflow;

namespace TypeCode.Business.Mode.Mapper
{
    internal class MappingContext : WorkflowBaseContext
    {
        public string Input { get; set; }
        public IEnumerable<string> TypeNames { get; set; }
        public List<MappingType> Types { get; set; }
        public List<int> ChoosenIndexes { get; set; }
        public string MappingCode { get; set; }
        public string MappingStyle { get; set; }
        public IEnumerable<IGrouping<string, MappingType>> TypesGrouped { get; set; }
        public List<MappingType> SelectedTypes { get; set; }
    }
}