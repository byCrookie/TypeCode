using System.Linq;
using System.Text;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.Mapper.Style
{
    internal class NewMapperStyleStrategy : INewMapperStyleStrategy
    {
        public int Number()
        {
            return 2;
        }
        
        public string Description()
        {
            return "Mapping To New";
        }

        public bool IsResponsibleFor(string style)
        {
            return style == $"{Number()}";
        }

        public string Generate(MappingContext context)
        {
            var firstType = context.SelectedFirstType;
            var secondType = context.SelectedSecondType;

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
            var methodName = secondType.EvaluateMethodName();

            stringBuilder.AppendLine($"public {secondType.Type.Name} {methodName}({firstType.Type.Name} {firstType.ParameterName()})");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"{Cuts.Tab()}return new {secondType.Type.Name}");
            stringBuilder.AppendLine($"{Cuts.Tab()}{{");
            foreach (var property in secondType.Properties())
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{property} = {firstType.ParameterName()}.{firstType.MostAccurateProperty(property)},");
            }

            RemoveLastComma(stringBuilder);

            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"{Cuts.Tab()}}}");

            stringBuilder.AppendLine("};");
        }
        
        private static void RemoveLastComma(StringBuilder stringBuilder)
        {
            stringBuilder.Length--;
            stringBuilder.Length--;
            stringBuilder.Length--;
        }
    }
}