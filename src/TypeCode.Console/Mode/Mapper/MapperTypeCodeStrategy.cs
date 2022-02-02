using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;
using Workflow;

namespace TypeCode.Console.Mode.Mapper;

internal class MapperTypeCodeStrategy : IMapperTypeCodeStrategy
{
    private readonly IWorkflowBuilder<MappingContext> _workflowBuilder;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> _mapperTypeCodeGenerator;

    public MapperTypeCodeStrategy(
        IWorkflowBuilder<MappingContext> workflowBuilder,
        ITypeProvider typeProvider,
        ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> mapperTypeCodeGenerator)
    {
        _workflowBuilder = workflowBuilder;
        _typeProvider = typeProvider;
        _mapperTypeCodeGenerator = mapperTypeCodeGenerator;
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
            .ReadLine(c => c.Input)
            .If(c => !string.IsNullOrEmpty(c.Input), c => c.TypeNames, c => c.Input?.Split(',').Select(split => split.Trim()).ToList())
            .Then(c => c.FirstTypeNames, c => _typeProvider.TryGetByName(c.TypeNames.First()))
            .Then(c => c.SelectedTypes, c => c.FirstTypeNames)
            .ThenAsync<IMultipleTypeSelectionStep<MappingContext>>()
            .Then(c => c.SelectedFirstType, c => new MappingType(c.SelectedType))
            .Then(c => c.SecondTypeNames, c => _typeProvider.TryGetByName(c.TypeNames.Last()))
            .Then(c => c.SelectedTypes, c => c.SecondTypeNames)
            .ThenAsync<IMultipleTypeSelectionStep<MappingContext>>()
            .Then(c => c.SelectedSecondType, c => new MappingType(c.SelectedType))
            .ThenAsync<ISelectionStep<MappingContext, SelectionStepOptions>,
                SelectionStepOptions>(config =>
                {
                    config.Selections = new List<string>
                    {
                        "Map to new",
                        "Map to existing"
                    };
                }
            )
            .IfFlow(context => context.Selection == 1, ifFlow => ifFlow
                .ThenAsync(c => c.MappingCode, c => _mapperTypeCodeGenerator.GenerateAsync(new MapperTypeCodeGeneratorParameter(c.SelectedFirstType, c.SelectedSecondType)
                {
                    MappingStyle = MappingStyle.New
                }))
            )
            .IfFlow(context => context.Selection == 2, ifFlow => ifFlow
                .ThenAsync(c => c.MappingCode, c => _mapperTypeCodeGenerator.GenerateAsync(new MapperTypeCodeGeneratorParameter(c.SelectedFirstType, c.SelectedSecondType)
                {
                    MappingStyle = MappingStyle.Existing
                }))
            )
            .Build();

        var workflowContext = await workflow.RunAsync(new MappingContext()).ConfigureAwait(false);
        return workflowContext.MappingCode;
    }

    public bool IsExit()
    {
        return false;
    }
}