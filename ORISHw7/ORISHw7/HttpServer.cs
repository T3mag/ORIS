using System.Net;
using MyHttpServer.Configuration;
using MyHttpServer.Handlers;
namespace MyHttpServer;
public class HttpServer {
    string prefix;
    HttpListener Listener { get; }
    StaticFilesHandler staticFileHandler = new StaticFilesHandler();
    Handler? controllersHandler = new ControllersHandler();
    private AppSettings Config { get;}
    public HttpServer(HttpListener listener) {
        Listener = listener;
        var appSettingLoader = new AppSettingsLoader();
        appSettingLoader.Init();
        Config = appSettingLoader.COnfigure!;
        prefix = Config.Address + ":" + Config.Port + "/";
    }
    public async Task Start() {
        try {
            Listener.Prefixes.Add(prefix);
            Listener.Start();
            Console.WriteLine("Запуск сервера");
            Task.Run(async () => {
                while (Listener.IsListening) {
                    var context = await Listener.GetContextAsync();
                    staticFileHandler.Incomer = controllersHandler;
                    staticFileHandler.HandleRequest(context);
                }
            });
            Console.WriteLine("Напиши 'stop' для остановки сервера.");
            await Task.Run(() => {
                while (true) if (Console.ReadLine()!.Equals("stop")) break;
            });
            Listener.Stop();
        } catch (Exception exception) {
            Console.WriteLine(exception.ToString());
        } finally {
            Console.WriteLine("Работа сервера завершена");
        }
    }
}