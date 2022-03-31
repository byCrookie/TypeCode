using System.Reflection;
using System.Text.Json;

namespace TypeCode.Business.Version;

public class VersionEvaluator : IVersionEvaluator
{
    public async Task<VersionResult> EvaluateAsync()
    {
        var currentVersion = await ReadCurrentVersionAsync().ConfigureAwait(false);
        var latestVersion = await GetVersionAsync().ConfigureAwait(false);

        if (!string.IsNullOrEmpty(currentVersion.CurrentVersion)
            && !string.IsNullOrEmpty(latestVersion?.TagName)
            && !string.Equals(currentVersion.CurrentVersion.Trim(), latestVersion.TagName.Trim(), StringComparison.CurrentCultureIgnoreCase))
        {
            return new VersionResult
            {
                CurrentVersion = currentVersion.CurrentVersion,
                NewVersion = latestVersion.TagName
            };
        }

        return new VersionResult
        {
            CurrentVersion = currentVersion.CurrentVersion
        };
    }

    public async Task<VersionResult> ReadCurrentVersionAsync()
    {
        var file = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\version";

        if (!File.Exists(file))
        {
            return new VersionResult();
        }

        var version = await File.ReadAllTextAsync(file).ConfigureAwait(false);
        return new VersionResult { CurrentVersion = version };
    }

    private static async Task<VersionResponse?> GetVersionAsync()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "byCrookie");

            var uri = new Uri("https://api.github.com/repos/byCrookie/TypeCode/releases");
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var releases = await JsonSerializer.DeserializeAsync<List<VersionResponse>>(stream).ConfigureAwait(false);
            return releases?.FirstOrDefault();
        }
    }
}