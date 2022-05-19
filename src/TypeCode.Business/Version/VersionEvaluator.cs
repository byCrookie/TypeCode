using System.Diagnostics;
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
            && !string.Equals(currentVersion.CurrentVersion.Trim(), GetVersionNumerFromTag(latestVersion), StringComparison.CurrentCultureIgnoreCase))
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

    public Task<VersionResult> ReadCurrentVersionAsync()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\TypeCode.Wpf.exe");
        var result = versionInfo.ProductVersion is null ? new VersionResult() : new VersionResult { CurrentVersion = versionInfo.ProductVersion };
        return Task.FromResult(result);
    }

    private static string GetVersionNumerFromTag(VersionResponse latestVersion)
    {
        var tag = latestVersion.TagName.Trim();
        return tag[1..];
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