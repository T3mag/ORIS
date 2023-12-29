using System.Text.Json;
namespace Server.AppSetting;
public class AppSettingsLoader {
    public static AppSettings Init(string pathConfigFile) {
        if (!File.Exists(pathConfigFile)) { 
            Console.WriteLine("appsettings.json не был обноружен");
            throw new Exception();
        } else {
            var file = File.OpenRead(pathConfigFile);
            var config = JsonSerializer.Deserialize<AppSettings>(file);
            return config;   
        }
    }
}