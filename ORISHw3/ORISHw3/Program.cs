using ORISHw3;
using System.Net;
using System.Text.Json;

string pathConfigFile = "/Users/arturminnusin/Desktop/ORIS/ORISHw3/ORISHw3/appsettings.json";
HttpListener listener = new HttpListener();
try {
    if (!File.Exists(pathConfigFile)) { 
        Console.WriteLine("Файл appsettings.json не был найден");
        throw new Exception();
    }
    AppSettings? config; ;
    config = JsonSerializer.Deserialize<AppSettings>(File.OpenRead(pathConfigFile));
    listener.Prefixes.Add(config.Address + ":" + config.Port + "/");
    listener.Start();
    Console.WriteLine("Сервер запущен");
    Task.Run(() => {
        while (listener.IsListening) {
            const string filePath = "/Users/arturminnusin/Desktop/ORIS/ORISHw3/ORISHw3/Index.html";
            var bytes = File.ReadAllBytes(filePath);
            var stream = listener.GetContext().Response.OutputStream;
            stream.Write(bytes);
            stream.Flush();
        }
    });
    Console.WriteLine("Напмшм 'stop' для остановки сервера.");
    await Task.Run(() => { while (true) if (Console.ReadLine()!.Equals("stop")) break;
    });
    listener.Stop(); } 
catch (Exception exception) { Console.WriteLine(exception.ToString()); } 
finally { Console.WriteLine("Работа сервера остановлена"); }