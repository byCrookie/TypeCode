using System.Text.RegularExpressions;

namespace TypeCode.Business;

public class ExclusionSplitter
{
    private readonly Regex _exclusionMarkerRegex;
    private readonly Regex _delimiterRegex;

    public ExclusionSplitter(string delimiter, string leftExclusion, string rightExclusion)
    {
        _exclusionMarkerRegex = new Regex($"{leftExclusion}{{1}}.*?{rightExclusion}{{1}}", RegexOptions.Compiled);
        _delimiterRegex = new Regex(delimiter, RegexOptions.Compiled);
    }
    
    public IEnumerable<string> Split(string content)
    {
        var markerMatches = _exclusionMarkerRegex
            .Matches(content)
            .ToList();

        var delimiterMatches = _delimiterRegex
            .Matches(content)
            .ToList();

        var statements = new List<string>();
        var lastIndex = 0;
        
        foreach (var delimiterMatch in delimiterMatches)
        {
            if (!markerMatches.Any(match => IsBetweenMarker(match, delimiterMatch.Index)))
            {
                statements.Add(content.Substring(lastIndex, delimiterMatch.Index - lastIndex));
                lastIndex = delimiterMatch.Index + 1;
            }
        }

        if (!string.IsNullOrEmpty(content[lastIndex..]))
        {
            statements.Add(content[lastIndex..]);
        }

        return statements;
    }

    private static bool IsBetweenMarker(Capture marker, int delimiterIndex)
    {
        return marker.Index < delimiterIndex && delimiterIndex < marker.Index + marker.Length;
    }
}