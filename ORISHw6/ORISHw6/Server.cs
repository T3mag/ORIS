using System.Net;
using Server.AppSetting;
using Server.handler;
namespace Server;
public class HttpServer {
    HttpListener Listener { get; }
    AppSettings Config { get; }
    string prefix;
    StaticFilesHandler Handler = new StaticFilesHandler();
    public HttpServer(HttpListener listener) {
        var appSettingLoader = new AppSettingsLoader().i;
        appSettingLoader.Init();
        Listener = listener;
        Config = appSettingLoader.Configuration!;
        prefix = Config.Address + ":" + Config.Port + "/";
    }
    public async Task Start() {
        try {
            Listener.Prefixes.Add(prefix);
            Listener.Start();
            Console.WriteLine("Сервер запущен");
            Task.Run(async () => {
                while (Listener.IsListening) {
                    await Handler.HandleRequest(await Listener.GetContextAsync());
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