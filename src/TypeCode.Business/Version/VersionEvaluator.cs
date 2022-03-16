using System.Reflection;
using System.Text.Json;

namespace TypeCode.Business.Version;

internal class VersionEvaluator : IVersionEvaluator
{
    public async Task<string?> EvaluateAsync()
    {
        var file = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\version";

        if (!File.Exists(file))
        {
            return null;
        }

        var version = await File.ReadAllTextAsync(file).ConfigureAwait(false);
        var latestVersion = await GetVersionAsync().ConfigureAwait(false);

        if (!string.IsNullOrEmpty(version)
            && !string.IsNullOrEmpty(latestVersion?.TagName)
            && !string.Equals(version.Trim(), latestVersion.TagName.Trim(), StringComparison.CurrentCultureIgnoreCase))
        {
            return latestVersion.TagName;
        }

        return null;
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