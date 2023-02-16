using System.Diagnostics;
using System.Text.Json;

namespace TypeCode.Business.Version;

public sealed class VersionEvaluator : IVersionEvaluator
{
    private readonly ISemanticVersionComparer _semanticVersionComparer;

    public VersionEvaluator(ISemanticVersionComparer semanticVersionComparer)
    {
        _semanticVersionComparer = semanticVersionComparer;
    }

    public async Task<VersionResult> EvaluateAsync()
    {
        var currentVersion = await ReadCurrentVersionAsync().ConfigureAwait(false);
        var newestVersion = await GetNewestVersionAsync().ConfigureAwait(false);

        if (HasNewerVersion(currentVersion, newestVersion))
        {
            return new VersionResult
            {
                CurrentVersion = currentVersion.CurrentVersion,
                NewVersion = newestVersion!.TagName
            };
        }

        return new VersionResult
        {
            CurrentVersion = currentVersion.CurrentVersion
        };
    }

    private bool HasNewerVersion(VersionResult currentVersion, VersionResponse? newestVersion)
    {
        if (string.IsNullOrEmpty(currentVersion.CurrentVersion) || string.IsNullOrEmpty(newestVersion?.TagName))
        {
            return false;
        }

        var current = Current(currentVersion);
        var newest = Newest(newestVersion);

        return _semanticVersionComparer.IsNewer(current, newest);
    }

    private static string Newest(VersionResponse newestVersion)
    {
        return GetVersionNumberFromTag(newestVersion).Trim();
    }

    private static string Current(VersionResult currentVersion)
    {
        return currentVersion.CurrentVersion!.Trim().Split('-').Length > 1
            ? currentVersion.CurrentVersion!.Trim().Split('-')[0]
            : currentVersion.CurrentVersion!.Trim();
    }

    public Task<VersionResult> ReadCurrentVersionAsync()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo($"{Path.GetDirectoryName(AppContext.BaseDirectory)}\\TypeCode.Wpf.exe");
        var result = versionInfo.ProductVersion is null ? new VersionResult() : new VersionResult { CurrentVersion = versionInfo.ProductVersion };
        return Task.FromResult(result);
    }

    private static string GetVersionNumberFromTag(VersionResponse latestVersion)
    {
        var tag = latestVersion.TagName.Trim();
        return tag[1..];
    }

    private static async Task<VersionResponse?> GetNewestVersionAsync()
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