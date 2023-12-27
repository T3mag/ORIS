using System.Net;
using MyHttpServer.Configuration;
using MyHttpServer.Handlers;
namespace MyHttpServer;
public class HttpServer {
    private HttpListener Listener { get; }
    private readonly string prefix;
    private readonly StaticFilesHandler staticFileHandler = new();
    private readonly ControllersHandler? controllersHandler = new();
    private AppSettings Config { get; }
    public HttpServer(HttpListener listener) {
        Listener = listener;
        var appSettingLoader = new AppSettingsLoader();
        appSettingLoader.Init();
        Config = appSettingLoader.Configuration!;
        prefix = Config.Address + ":" + Config.Port + "/";
    }
    public async Task Start() {
        try {
            Listener.Prefixes.Add(prefix);
            Listener.Start();
            Console.WriteLine("Server started");
            Task.Run(async () => {
                while (Listener.IsListening) {
                    var context = await Listener.GetContextAsync();
                    staticFileHandler.Successor = controllersHandler;
                    staticFileHandler.HandleRequest(context);
                }
            });
            Console.WriteLine("Пиши 'stop' для остановки.");
            await Task.Run(() => {
                while (true) if (Console.ReadLine()!.Equals("stop")) break;
            });
            Listener.Stop();
        } catch (Exception exception) { Console.WriteLine(exception.ToString());
        } finally {
            Console.WriteLine("Работа сервера завершена");
        }
    }
}