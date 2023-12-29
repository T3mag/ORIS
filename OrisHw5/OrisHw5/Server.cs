using System.Net;
using System.Text;
using Server.AppSetting;
namespace Server;
public class HttpServer {
    HttpListener Listener { get;}
    AppSettings SettingsApp { get; }
    string config = "/Users/arturminnusin/Desktop/ORIS/OrisHw5/OrisHw5/appsettings.json";
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
                    var path = context.Request.Url!.LocalPath;
                    if (!path.EndsWith(".html") || path.Equals("/")) {
                        var fileName = "index.html";
                        if (Directory.Exists(SettingsApp.StaticFilesPath)) {
                            var filePath = Path.Combine(SettingsApp.StaticFilesPath!, fileName);
                            if (File.Exists(filePath)) {
                                var buffer = await File.ReadAllBytesAsync(filePath);
                                var output = response.OutputStream;
                                output.WriteAsync(buffer);
                                output.FlushAsync();
                            }
                            else {
                                Console.WriteLine($"Файл {fileName} не был обноружен");
                                var errorMessege = "<h2>Внимание</h2><h3>Нет файла</h3>";
                                var buffer = Encoding.UTF8.GetBytes(errorMessege);
                                var output = response.OutputStream;
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