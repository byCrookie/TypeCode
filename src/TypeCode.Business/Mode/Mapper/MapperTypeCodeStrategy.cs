using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.Mode.Mapper.Style;
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
            return true;
        }

        public bool IsResponsibleFor(string mode)
        {
            return mode == $"{Number()}" && !IsPlanned();
        }

        public async Task<string> GenerateAsync()
        {
            var workflow = _workflowBuilder
                .WriteLine(context => $@"{Cuts.Point()} Seperate two types by ,")
                .Then(context => context.Input, context => Console.ReadLine())
                .If(context => !string.IsNullOrEmpty(context.Input), context => context.TypeNames, context => context.Input?.Split(',').Select(split => split.Trim()).ToList())
                .If(context => context.TypeNames.Any(), context => context.Types, context => _typeProvider.TryGetByNames(context.TypeNames).Select(typ => new MappingType {Type = typ}).ToList())
                .Then(context => context.TypesGrouped, context => context.Types.GroupBy(type => type.DataType()))
                .Then(context => context.SelectedTypes, context => new List<MappingType>())
                .IfElseFlow(context => context.TypesGrouped.Any(group => group.Count() > 1),
                    ifFlow => ifFlow
                        .IfElseFlow(context => context.TypesGrouped.First().Count() > 1,
                            subIfFlow => subIfFlow
                                .WriteLine(context => $@"{Cuts.Heading()} Multiple types found, select by seperating numbers using ,")
                                .Then(context => WriteTypesToScreen(context.TypesGrouped.First()))
                                .Then(context => context.ChoosenIndexes, context => Console.ReadLine()?.Split(',').Select(split => Convert.ToInt32(split.Trim())).ToList())
                                .Then(context => context.SelectedTypes.Add(context.Types.Single(type => context.ChoosenIndexes.Contains(context.Types.IndexOf(type))))),
                            subElseFlow => subElseFlow
                                .Then(context => context.SelectedTypes.Add(context.TypesGrouped.First().Single()))
                        )
                        .IfElseFlow(context => context.TypesGrouped.Last().Count() > 1,
                            subIfFlow => subIfFlow
                                .WriteLine(context => $@"{Cuts.Heading()} Multiple types found, select by seperating numbers using ,")
                                .Then(context => WriteTypesToScreen(context.TypesGrouped.Last()))
                                .Then(context => context.ChoosenIndexes, context => Console.ReadLine()?.Split(',').Select(split => Convert.ToInt32(split.Trim())).ToList())
                                .Then(context => context.SelectedTypes.Add(context.Types.Single(type => context.ChoosenIndexes.Contains(context.Types.IndexOf(type))))),
                            subElseFlow => subElseFlow
                                .Then(context => context.SelectedTypes.Add(context.TypesGrouped.Last().Single()))
                        ),
                    elseFlow => elseFlow
                        .Then(context => context.SelectedTypes, context => context.Types))
                .Then(WriteMappingStyleToScreen)
                .Then(context => context.MappingStyle, context => Console.ReadLine()?.Trim())
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

        private static void WriteTypesToScreen(IGrouping<string, MappingType> context)
        {
            for (var index = 0; index < context.Count(); index++)
            {
                Console.WriteLine($@"{Cuts.Point()} {index + 1} - {context.ToList()[index].Type.FullName}");
            }
        }
    }
}