namespace TypeCode.Business.Mode.Guid;

public sealed class GuidCodeGenerator : IGuidCodeGenerator
{
    public Task<string?> GenerateAsync(GuidTypeCodeGeneratorParameter parameter)
    {
        return Task.FromResult<string?>(GenerateGuid(parameter));
    }

    private static string GenerateGuid(GuidTypeCodeGeneratorParameter parameter)
    {
        return parameter.Format switch
        {
            GuidFormat.N => System.Guid.NewGuid().ToString("N"),
            GuidFormat.D => System.Guid.NewGuid().ToString("D"),
            GuidFormat.B => System.Guid.NewGuid().ToString("B"),
            GuidFormat.P => System.Guid.NewGuid().ToString("P"),
            GuidFormat.X => System.Guid.NewGuid().ToString("X"),
            _ => throw new ArgumentOutOfRangeException(nameof(parameter.Format))
        };
    }
}