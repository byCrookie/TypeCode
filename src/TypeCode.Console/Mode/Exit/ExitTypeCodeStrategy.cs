namespace TypeCode.Console.Mode.Exit;

internal class ExitTypeCodeStrategy : IExitTypeCodeStrategy
{
    public int Number()
    {
        return 6;
    }

    public string Description()
    {
        return "Exit";
    }

    public bool IsPlanned()
    {
        return false;
    }

    public bool IsBeta()
    {
        return false;
    }

    public bool IsResponsibleFor(string? mode)
    {
        return mode is not null && mode == $"{Number()}" && !IsPlanned();
    }

    public Task<string?> GenerateAsync()
    {
        return Task.FromResult<string?>(null);
    }

    public bool IsExit()
    {
        return true;
    }
}