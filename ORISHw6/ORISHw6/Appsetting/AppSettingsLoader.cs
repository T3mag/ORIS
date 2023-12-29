using System.Text.Json;

namespace Server.AppSetting;

public class AppSettingsLoader {
    public AppSettings? Configuration { get; private set; }
    string Path { get; }
    static bool chekerInitialized;
    static AppSettingsLoader? Sigelton;
    public AppSettingsLoader() => Path = "/Users/arturminnusin/Desktop/ORIS/ORISHw6/ORISHw6/appsettings.json";
    private AppSettingsLoader(string path, AppSettings config) {
        Path = path; Configuration = config;
    }
    public void Init() {
        var json = File.ReadAllText(Path);
        Configuration = JsonSerializer.Deserialize<AppSettings>(json);
        chekerInitialized = true;
        Sigelton = new AppSettingsLoader(Path, Configuration!);
        if (!File.Exists(Path)) throw new ArgumentException("Файл appsettings.json не обноружен");
    }
    public static AppSettingsLoader? Instance() {
        if (chekerInitialized) return Sigelton; throw new InvalidOperationException("DataServer Singleton is not initialized");
    }
}