using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;

namespace TypeCode.Console.Mode.UnitTestDependency
{
    internal class UnitTestDependencyTypeCodeStrategy : IUnitTestDependencyTypeCodeStrategy
    {
        private readonly IWorkflowBuilder<UnitTestDependencyEvaluationContext> _workflowEvaluationBuilder;
        private readonly ITypeProvider _typeProvider;
        private readonly ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> _unitTestDependencyTypeGenerator;
        private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;

        public UnitTestDependencyTypeCodeStrategy(
            IWorkflowBuilder<UnitTestDependencyEvaluationContext> workflowEvaluationBuilder,
            ITypeProvider typeProvider,
            ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> unitTestDependencyTypeGenerator,
            ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator
        )
        {
            _workflowEvaluationBuilder = workflowEvaluationBuilder;
            _typeProvider = typeProvider;
            _unitTestDependencyTypeGenerator = unitTestDependencyTypeGenerator;
            _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;
        }

        public int Number()
        {
            return 2;
        }

        public string Description()
        {
            return "Unit Test Fake Generation";
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
            var workflow = _workflowEvaluationBuilder
                .ThenAsync<ISelectionStep<UnitTestDependencyEvaluationContext, SelectionStepOptions>,
                    SelectionStepOptions>(config =>
                    {
                        config.Selections = new List<string>
                        {
                            "Input type by name",
                            "Input dependencies manually"
                        };
                    }
                )
                .IfFlow(context => context.Selection == 1, ifFlow => ifFlow
                    .WriteLine(_ => $@"{Cuts.Point()} Please input type name")
                    .ReadLine(context => context.Input)
                    .While(context => !_typeProvider.HasByName(context.Input.Trim()), whileFlow => whileFlow
                        .WriteLine(_ => $@"{Cuts.Point()} Type not found")
                        .WriteLine(_ => $@"{Cuts.Point()} Input type name")
                        .ReadLine(context => context.Input)
                        .ThenAsync<IExitOrContinueStep<UnitTestDependencyEvaluationContext>>()
                    )
                    .If(context => !string.IsNullOrEmpty(context.Input), context => context.SelectedTypes,
                        context => _typeProvider.TryGetByName(context.Input.Trim()))
                    .ThenAsync<IMultipleTypeSelectionStep<UnitTestDependencyEvaluationContext>>()
                    .ThenAsync(context => context.UnitTestDependencyCode,
                        context => _unitTestDependencyTypeGenerator.GenerateAsync(new UnitTestDependencyTypeGeneratorParameter
                        {
                            Types = context.SelectedTypes.ToList()
                        }))
                )
                .IfFlow(context => context.Selection == 2, ifFlow => ifFlow
                    .WriteLine(_ => $@"{Cuts.Point()} Please input constructor as text with last character ')'")
                    .ReadMultiLine(context => context.Input, options =>
                    {
                        options.EndOfInput = ")";
                        options.RemoveEndOfInput = false;
                        options.ShouldTrimLines = true;
                    })
                    .ThenAsync(context => context.UnitTestDependencyCode,
                        context => _unitTestDependencyManuallyGenerator.GenerateAsync(
                            new UnitTestDependencyManuallyGeneratorParameter { Input = context.Input })))
                .Build();

            var workflowContext =
                await workflow.RunAsync(new UnitTestDependencyEvaluationContext()).ConfigureAwait(false);
            return workflowContext.UnitTestDependencyCode;
        }

        public bool IsExit()
        {
            return false;
        }
    }
}