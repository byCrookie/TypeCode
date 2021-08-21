using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.Mode.MultipleTypes;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Mode.Mapper
{
    internal class MapperTypeCodeStrategy : IMapperTypeCodeStrategy
    {
        private readonly IWorkflowBuilder<MappingContext> _workflowBuilder;
        private readonly IMapperStyleComposer _mapperStyleComposer;
        private readonly ITypeProvider _typeProvider;

        public MapperTypeCodeStrategy(
            IWorkflowBuilder<MappingContext> workflowBuilder,
            IMapperStyleComposer mapperStyleComposer,
            ITypeProvider typeProvider)
        {
            _workflowBuilder = workflowBuilder;
            _mapperStyleComposer = mapperStyleComposer;
            _typeProvider = typeProvider;
        }

        public int Number()
        {
            return 4;
        }

        public string Description()
        {
            return "Mapper Generation";
        }

        public bool IsPlanned()
        {
            return false;
        }

        public bool IsBeta()
        {
            return false;
        }

        public bool IsResponsibleFor(string mode)
        {
            return mode == $"{Number()}" && !IsPlanned();
        }

        public async Task<string> GenerateAsync()
        {
            var workflow = _workflowBuilder
                .WriteLine(_ => $@"{Cuts.Point()} Seperate two types by ,")
                .Then(context => context.Input, _ => Console.ReadLine())
                .If(context => !string.IsNullOrEmpty(context.Input), context => context.TypeNames, context => context.Input?.Split(',').Select(split => split.Trim()).ToList())
                .Then(context => context.FirstTypeNames, context => _typeProvider.TryGetByName(context.TypeNames.First()))
                .Then(context => context.SelectedTypes, context => context.FirstTypeNames)
                .ThenAsync<IMultipleTypeSelectionStep<MappingContext>>()
                .Then(context => context.SelectedFirstType, context => new MappingType(context.SelectedType))
                .Then(context => context.SecondTypeNames, context => _typeProvider.TryGetByName(context.TypeNames.Last()))
                .Then(context => context.SelectedTypes, context => context.SecondTypeNames)
                .ThenAsync<IMultipleTypeSelectionStep<MappingContext>>()
                .Then(context => context.SelectedSecondType, context => new MappingType(context.SelectedType))
                .Then(WriteMappingStyleToScreen)
                .ReadLine(context => context.MappingStyle)
                .Then(context => context.MappingCode, GenerateMappingCode)
                .Build();

            var workflowContext = await workflow.RunAsync(new MappingContext()).ConfigureAwait(false);
            return workflowContext.MappingCode;
        }

        private void WriteMappingStyleToScreen(MappingContext context)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            foreach (var style in _mapperStyleComposer.ComposeOrdered())
            {
                stringBuilder.AppendLine($"{Cuts.Point()} {style.Number()} - {style.Description()}");
            }
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.Append($@"{Cuts.Point()} Select a style");
            Console.WriteLine(stringBuilder.ToString());
        }

        private string GenerateMappingCode(MappingContext context)
        {
            var styles = _mapperStyleComposer.ComposeOrdered();
            var selectedStyle = styles.SingleOrDefault(style => style.IsResponsibleFor(context.MappingStyle));
            return selectedStyle?.Generate(context) ?? string.Empty;
        }
    }
}