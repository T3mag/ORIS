using System.Text.Json;

namespace MyHttpServer.Configuration;

public class AppSettingsLoader {
    private string Path { get;}
    public AppSettings? COnfigure { get; private set; }
    static bool cheker;
    static AppSettingsLoader? sigelton;
    public AppSettingsLoader() => 
        Path = $"../../../appsettings.json";
    private AppSettingsLoader(string path, AppSettings config) { Path = path; COnfigure = config; }
    public void Init() {
        try {
            var json = File.ReadAllText(Path);
            COnfigure = JsonSerializer.Deserialize<AppSettings>(json);
            cheker = true;
            sigelton = new AppSettingsLoader(Path, COnfigure!);
        } catch (Exception exception) {
            Console.WriteLine(exception);
        }
        if (!File.Exists(Path)) throw new ArgumentException("appsettings.json не найден");
    }
    public static AppSettingsLoader? Instance() {
        if (cheker) return sigelton;
        throw new InvalidOperationException("Ошибка");
    }
}