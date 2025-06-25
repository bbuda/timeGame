using System.IO;
using System.Text.Json;

public static class SettingsService
{
    private const string FileName = "settings.json";

    public static SettingsModel Load()
    {
        if (File.Exists(FileName))
        {
            try
            {
                var json = File.ReadAllText(FileName);
                var settings = JsonSerializer.Deserialize<SettingsModel>(json);
                if (settings != null)
                    return settings;
            }
            catch
            {
                // ignore errors and fallback to defaults
            }
        }
        return new SettingsModel();
    }

    public static void Save(SettingsModel settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }
}
