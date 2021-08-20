﻿using System.Linq;
using System.Text;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.Mapper.Style
{
    internal class ExistingMapperStyleStrategy : IExistingMapperStyleStrategy
    {
        public int Number()
        {
            return 1;
        }

        public string Description()
        {
            return "Mapping To Existing";
        }

        public bool IsResponsibleFor(string style)
        {
            return style == $"{Number()}";
        }

        public string Generate(MappingContext context)
        {
            var firstType = context.SelectedTypes.First();
            var secondType = context.SelectedTypes.Last();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Cuts.Long()}");
            stringBuilder.AppendLine($"{Cuts.Heading()} {secondType.Type.Name} -> {firstType.Type.Name}, {secondType.Type.FullName} -> {firstType.Type.FullName}");
            stringBuilder.AppendLine();
            GenerateUsingStyle(stringBuilder, secondType, firstType);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"{Cuts.Heading()} {firstType.Type.Name} -> {secondType.Type.Name}, {firstType.Type.FullName} -> {secondType.Type.FullName}");
            stringBuilder.AppendLine();
            GenerateUsingStyle(stringBuilder, firstType, secondType);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"{Cuts.Long()}");
            return stringBuilder.ToString();
        }
        
        private static void GenerateUsingStyle(StringBuilder stringBuilder, MappingType firstType, MappingType secondType)
        {
            var methodName = MappingType.EvaluateMethodName(secondType);

            stringBuilder.AppendLine($"public void {methodName}({firstType.Type.Name} {firstType.ParameterName()}, {secondType.Type.Name} {secondType.ParameterName()})");
            stringBuilder.AppendLine("{");
            foreach (var property in secondType.Properties())
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}{secondType.ParameterName()}.{property} = {firstType.ParameterName()}.{firstType.MostAccurateProperty(property)};");
            }

            stringBuilder.AppendLine("}");
        }
    }
}