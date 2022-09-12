namespace TypeCode.Business.Version;

public sealed class SemanticVersionComparer : ISemanticVersionComparer
{
    public bool IsNewer(string current, string newest)
    {
        var currentSplitted = current.Split(".").Select(int.Parse);
        var newestSplitted = newest.Split(".").Select(int.Parse);
        var zipped = currentSplitted.Zip(newestSplitted).Reverse().ToList();

        var isHigher = false;
        foreach (var (currentPart, newestPart) in zipped)
        {
            if (newestPart > currentPart)
            {
                isHigher = true;
            }
            else if (newestPart < currentPart)
            {
                isHigher = false;
            }
        }

        return isHigher;
    }
}