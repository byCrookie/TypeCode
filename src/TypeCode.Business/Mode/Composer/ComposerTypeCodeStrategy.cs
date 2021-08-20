using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.Mode.MultipleTypes;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Mode.Composer
{
    internal class ComposerTypeCodeStrategy : IComposerTypeCodeStrategy
    {
        private readonly IWorkflowBuilder<ComposerContext> _workflowBuilder;
        private readonly ITypeProvider _typeProvider;

        public ComposerTypeCodeStrategy(
            IWorkflowBuilder<ComposerContext> workflowBuilder,
            ITypeProvider typeProvider)
        {
            _workflowBuilder = workflowBuilder;
            _typeProvider = typeProvider;
        }

        public int Number()
        {
            return 3;
        }

        public string Description()
        {
            return "Composer Generation";
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
                .WriteLine(context => $@"{Cuts.Point()} Input strategy interface")
                .Then(context => context.TypeName, context => Console.ReadLine())
                .While(context => !_typeProvider.HasByName(context.TypeName.Trim()), whileFlow => whileFlow
                    .WriteLine(context => $@"{Cuts.Point()} Interface not found")
                    .WriteLine(context => $@"{Cuts.Point()} Please input strategy interface")
                    .Then(context => context.TypeName, context => Console.ReadLine())
                    .IfFlow(context => string.IsNullOrEmpty(context.TypeName), ifFlow => ifFlow
                        .WriteLine(context => $@"{Cuts.Point()} Press enter to exit or space to continue")
                        .IfFlow(context => Console.ReadKey().Key == ConsoleKey.Enter, ifFlowLeave => ifFlowLeave
                            .StopAsync()
                        )
                    )
                )
                .Then(context => context.SelectedTypes, context => _typeProvider.TryGetByName(context.TypeName.Trim()).ToList())
                .ThenAsync<IMultipleTypeSelectionStep<ComposerContext>>()
                .Stop(c => !c.SelectedType.IsInterface, c => Console.WriteLine($@"{Cuts.Point()} Type has to be an interface"))
                .Then(context => context.ComposerCode, context => GenerateComposerCode(context.SelectedType))
                .Build();

            var workflowContext = await workflow.RunAsync(new ComposerContext()).ConfigureAwait(false);
            return workflowContext.ComposerCode;
        }

        private string GenerateComposerCode(Type typeInterface)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.AppendLine($@"{Cuts.Heading()} {typeInterface.FullName}");

            var strategyTypes = _typeProvider
                .TryGetTypesByCondition(typ => typ.GetInterface(typeInterface.Name) != null)
                .ToList();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine(@"private IFactory _factory;");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine(@"public Composer(IFactory factory)");
            stringBuilder.AppendLine(@"{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}_factory = factory");
            stringBuilder.AppendLine(@"}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($@"public IEnumerable<{NameBuilder.GetNameWithoutGeneric(typeInterface)}> Compose()");
            stringBuilder.AppendLine(@"{");
            foreach (var strategyType in strategyTypes)
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}yield return _factory.Create<{NameBuilder.GetInterfaceName(strategyType)}>();");
            }

            stringBuilder.AppendLine(@"}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($@"{Cuts.Long()}");

            return stringBuilder.ToString();
        }

        
    }
}