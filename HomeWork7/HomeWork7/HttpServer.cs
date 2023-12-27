using System.Net;
using Server.Configuration;
using Server.Handlers;
namespace MyHttpServer;
public class HttpServer {
    private HttpListener Listener { get; }
    private readonly string _prefix;
    private readonly StaticFilesHandler _staticFileHandler = new();
    private readonly Handler? _controllersHandler = new ControllersHandler();
    private AppSettings Config { get; set; }
    public HttpServer(HttpListener listener)  {
        Listener = listener;
        var appSettingLoader = new AppSettingsLoader();
        appSettingLoader.Init();
        Config = appSettingLoader.Configuration!;
        _prefix = Config.Address + ":" + Config.Port + "/";
    }
    public async Task Start() {
        try {
            Listener.Prefixes.Add(_prefix);
            Listener.Start();
            Console.WriteLine("Server started");
            Task.Run(async () =>
            {
                while (Listener.IsListening) {
                    var context = await Listener.GetContextAsync();
                    _staticFileHandler.Successor = _controllersHandler;
                    _staticFileHandler.HandleRequest(context);
                }
            });
            Console.WriteLine("Напиши 'stop' для остановки сервера");
            await Task.Run(() => {
                while (true)
                    if (Console.ReadLine()!.Equals("stop")) break;
            });
            Listener.Stop();
        }
        catch (Exception exception) {
            C