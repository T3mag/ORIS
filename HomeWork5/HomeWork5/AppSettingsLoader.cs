using System.Text.Json;
namespace Server.Configuration;
public class AppSettingsLoader {
    public static AppSettings Init(string pathConfigFile) {
        if (!File.Exists(pathConfigFile)) { 
            Console.WriteLine("Файл appsettings.json не обноружен");
            throw new Exception();
        }
        using var file = File.OpenRead(pathConfigFile);
        var config = JsonSerializer.Deserialize<AppSettings>(file);
        return config!;
    }
}