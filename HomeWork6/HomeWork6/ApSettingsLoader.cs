using System.Text.Json;
namespace Server.Configuration;
public class AppSettingsLoader
{
    private string Path { get; set; }
    public AppSettings? Configuration { get; private set; }

    private static bool cheker;
    
    private static AppSettingsLoader? _instance;

    public const string CurDirectory = "../../../";

    public AppSettingsLoader() => Path = $"{CurDirectory}appsettings.json";
    private AppSettingsLoader(string path, AppSettings config) {
        Path = path;
        Configuration = config;
    }
    public void Init() {
        try {
            var json = File.ReadAllText(Path);
            Configuration = JsonSerializer.Deserialize<AppSettings>(json);
            cheker = true;
            _instance = new AppSettingsLoader(Path, Configuration!);
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw;
        }
        if (!File.Exists(Path)) throw new ArgumentException("appsettings.json не обноружен");
    }
    public static AppSettingsLoader? Instance() {
        if (cheker) return _instance;
        throw new InvalidOperationException("DataServer Singleton is not initialized");
    }
}