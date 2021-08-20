using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.Mode.MultipleTypes;
using TypeCode.Business.Mode.Selection;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Mode.UnitTestDependency
{
    internal class UnitTestDependencyTypeCodeStrategy : IUnitTestDependencyTypeCodeStrategy
    {
        private readonly IWorkflowBuilder<UnitTestDependencyEvaluationContext> _workflowEvaluationBuilder;
        private readonly IWorkflowBuilder<UnitTestDependencyGenerationContext> _workflowGenerationBuilder;
        private readonly ITypeProvider _typeProvider;

        public UnitTestDependencyTypeCodeStrategy(
            IWorkflowBuilder<UnitTestDependencyEvaluationContext> workflowEvaluationBuilder,
            IWorkflowBuilder<UnitTestDependencyGenerationContext> workflowGenerationBuilder,
            ITypeProvider typeProvider
        )
        {
            _workflowEvaluationBuilder = workflowEvaluationBuilder;
            _workflowGenerationBuilder = workflowGenerationBuilder;
            _typeProvider = typeProvider;
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
                            "Input dependencies manually (,)"
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
                    )
                    .ThenAsync(context => context.UnitTestDependencyCode,
                        context => GenerateUnitTestDependenciesAsync(context.Input))
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
                        context => GenerateUnitTestDependenciesManuallyAsync(context.Input)))
                .Build();

            var workflowContext =
                await workflow.RunAsync(new UnitTestDependencyEvaluationContext()).ConfigureAwait(false);
            return workflowContext.UnitTestDependencyCode;
        }

        private static Task<string> GenerateUnitTestDependenciesManuallyAsync(string input)
        {
            var lines = input.Split(Environment.NewLine);

            return lines.Length > 1
                ? GenerateForMultiLineAsync(lines)
                : GenerateForSingleLineAsync(lines.Single());
        }

        private static Task<string> GenerateForMultiLineAsync(IReadOnlyList<string> lines)
        {
            return Task.FromResult(string.Empty);
        }

        private static readonly Regex PartsRegex =
            new(@"\(([^)]*)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(300));

        private static Task<string> GenerateForSingleLineAsync(string line)
        {
            var matches = PartsRegex.Split(line);
            var accessorAndName = matches[0];
            var dependencies = matches[1];

            var className = accessorAndName.Split(" ").Skip(1).Single();
            var dependenciesCommaSeperated = dependencies.Split(",").Select(dependency => dependency.Trim());
            var dependenciesByTypeAndName = dependenciesCommaSeperated
                .Select(dependency => new DependencyManually(dependency))
                .ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.AppendLine($@"{Cuts.Heading()} {className}");

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($@"private {className} _testee;");
            stringBuilder.AppendLine();

            foreach (var dependency in dependenciesByTypeAndName)
            {
                stringBuilder.AppendLine(
                    $@"private {dependency.TypeName} _{dependency.Name};");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("[TestInitialize]");
            stringBuilder.AppendLine("public void TestInitialize()");
            stringBuilder.AppendLine("{");

            foreach (var dependency in dependenciesByTypeAndName)
            {
                stringBuilder.AppendLine(
                    $@"{Cuts.Tab()}_{dependency.Name} = A.Fake<{dependency.TypeName}>();");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"{Cuts.Tab()}_testee = new {className}(");
            foreach (var dependency in dependenciesByTypeAndName)
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}_{dependency.Name},");
            }

            RemoveLastComma(stringBuilder);

            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"{Cuts.Tab()});");
            stringBuilder.AppendLine("}");

            stringBuilder.AppendLine();

            stringBuilder.Append($@"{Cuts.Long()}");

            return Task.FromResult(stringBuilder.ToString());
        }

        private async Task<string> GenerateUnitTestDependenciesAsync(string input)
        {
            var workflow = _workflowGenerationBuilder
                .If(_ => !string.IsNullOrEmpty(input), context => context.SelectedTypes,
                    context => _typeProvider.TryGetByName(input.Trim()))
                .ThenAsync<IMultipleTypeSelectionStep<UnitTestDependencyGenerationContext>>()
                .IfElse(context => context.SelectedType is null,
                    _ => Console.WriteLine($@"{Cuts.Point()} Type not found"),
                    context => context.EvaluatedConstructors, context => context.SelectedType.GetConstructors()
                )
                .IfElse(context => !context.EvaluatedConstructors.Any(),
                    _ => Console.WriteLine($@"{Cuts.Point()} Type has no constructors"),
                    context => context.EvaluatedConstructor,
                    context => context.EvaluatedConstructors.OrderByDescending(ctor => ctor.GetParameters()).First()
                )
                .IfElse(context => context.EvaluatedConstructor is null,
                    _ => Console.WriteLine($@"{Cuts.Point()} Type has constructors"),
                    context => context.Parameters, context => context.EvaluatedConstructor.GetParameters()
                )
                .IfElse(context => !context.Parameters.Any(),
                    _ => Console.WriteLine($@"{Cuts.Point()} Type has no constructor with dependencies"),
                    context => context.DependencyCode, GenerateDependencies
                )
                .Build();

            var workflowContext =
                await workflow.RunAsync(new UnitTestDependencyGenerationContext()).ConfigureAwait(false);
            return workflowContext.DependencyCode;
        }

        private static string GenerateDependencies(UnitTestDependencyGenerationContext context)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.AppendLine($@"{Cuts.Heading()} {context.EvaluatedConstructor.DeclaringType?.FullName}");

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($@"private {context.EvaluatedConstructor.DeclaringType?.Name} _testee;");
            stringBuilder.AppendLine();

            foreach (var param in context.Parameters)
            {
                stringBuilder.AppendLine(
                    $@"private {NameBuilder.GetInterfaceName(param.ParameterType)} _{param.Name};");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("[TestInitialize]");
            stringBuilder.AppendLine("public void TestInitialize()");
            stringBuilder.AppendLine("{");

            foreach (var param in context.Parameters)
            {
                stringBuilder.AppendLine(
                    $@"{Cuts.Tab()}_{param.Name} = A.Fake<{NameBuilder.GetInterfaceName(param.ParameterType)}>();");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"{Cuts.Tab()}_testee = new {context.SelectedType.Name}(");
            foreach (var param in context.Parameters)
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}_{param.Name},");
            }

            RemoveLastComma(stringBuilder);

            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"{Cuts.Tab()});");
            stringBuilder.AppendLine("}");

            stringBuilder.AppendLine();

            stringBuilder.Append($@"{Cuts.Long()}");
            return stringBuilder.ToString();
        }

        private static void RemoveLastComma(StringBuilder stringBuilder)
        {
            stringBuilder.Length--;
            stringBuilder.Length--;
            stringBuilder.Length--;
        }
    }

    internal class DependencyManually
    {
        public DependencyManually(string dependency)
        {
            var splitted = dependency.Split(" ");
            TypeName = splitted[0];
            Name = splitted[1];
        }

        public string Name { get; }
        public string TypeName { get; }
    }
}