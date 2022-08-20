using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace TypeCode.Business.Version;

public class VersionEvaluator : IVersionEvaluator
{
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

    private static bool HasNewerVersion(VersionResult currentVersion, VersionResponse? newestVersion)
    {
        if (string.IsNullOrEmpty(currentVersion.CurrentVersion) || string.IsNullOrEmpty(newestVersion?.TagName))
        {
            return false;
        }

        var current = int.Parse(string.Join("", currentVersion.CurrentVersion.Trim().Split('.')[..3]));
        var newest = int.Parse(string.Join("", GetVersionNumberFromTag(newestVersion).Trim().Split('.')[..3]));
        
        return current < newest;
    }

    public Task<VersionResult> ReadCurrentVersionAsync()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\TypeCode.Wpf.exe");
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