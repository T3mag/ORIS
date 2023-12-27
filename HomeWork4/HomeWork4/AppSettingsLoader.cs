using System.Text.Json;
namespace Server.Configuration;
public class AppSettingsLoader {
    public static AppSettings Init(string path) {
        if (!File.Exists(path)) { 
            Console.WriteLine("Файл appsettings.json не обноружен");
            throw new Exception();
        }
        using var file = File.OpenRead(path);
        var config = JsonSerializer.Deserialize<AppSettings>(file);
        return config!;
    }
}