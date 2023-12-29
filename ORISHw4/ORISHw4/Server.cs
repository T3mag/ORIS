using System.Net;
using System.Text;
using Server.AppSetting;
namespace Server;
public class HttpServer {
    private HttpListener Listener { get;}
    private const string config = "/Users/arturminnusin/Desktop/ORIS/ORISHw4/ORISHw4/appsettings.json";
    private AppSettings SettingsApp { get; }
    public HttpServer(HttpListener listener) {
        Listener = listener;
        SettingsApp = AppSettingsLoader.Init(config);
    }
    public async Task Start() {
        try {
            Listener.Prefixes.Add(SettingsApp.Address + ":" + SettingsApp.Port + "/");
            Listener.Start();
            Console.WriteLine("Сервер запущен");
            Task.Run(async () => {
                while (Listener.IsListening) {
                    var context = await Listener.GetContextAsync();
                    var response = context.Response;
                    var localPath = context.Request.Url!.LocalPath;
                    if (!localPath.EndsWith(".html") || localPath.Equals("/")) {
                        const string fileName = "index.html";
                        if (Directory.Exists(SettingsApp.StaticFilesPath)) {
                            var filePath = Path.Combine(SettingsApp.StaticFilesPath!, fileName);
                            if (File.Exists(filePath)) {
                                var buffer = await File.ReadAllBytesAsync(filePath);
                                await using var output = response.OutputStream;
                                await output.WriteAsync(buffer);
                                await output.FlushAsync();
                            }
                            else {
                                Console.WriteLine($"Файл {fileName} не был обноружен");
                                string errorMessege = "<h2>Внимание</h2><h3>Файл не найден</h3>";
                                response.ContentType = "text/html; charset=utf-8";
                                var buffer = Encoding.UTF8.GetBytes(errorMessege);
                                await using var output = response.OutputStream;
                                await output.WriteAsync(buffer);
                                await output.FlushAsync();
                            }
                        } 
                    } 
                }
            });
        
            Console.WriteLine("Напишите 'stop' для остановки сервера.");
        
            await Task.Run(() => {
                while (true) if (Console.ReadLine()!.Equals("stop")) break;
            });
            Listener.Stop();
        } finally {
            Console.WriteLine("Работа сервера завершена");
        }
    }
}