using System.Net;
using System.Text;
using MyHttpServer.Configuration;
namespace MyHttpServer.Handlers;
public class StaticFilesHandler: Handler {
    public override async void HandleRequest(HttpListenerContext context) {
        var config = AppSettingsLoader.Instance();
        IsDirectoryExistAndCreate("../../../" + config!.Configuration!.StaticFilesPath);
        var response = context.Response;
        var request = context.Request;
        var localPath = request.Url!.LocalPath;
        if (localPath is "" or "/") localPath = "/index.html";
        if (localPath.Contains('.')) {
            var filePath = "../../../" + config.Configuration.StaticFilesPath + localPath;
            if (File.Exists(filePath)) {
                var buffer = await File.ReadAllBytesAsync(filePath);
                response.ContentLength64 = buffer.Length;
                response.ContentType = GetContentType(localPath);
                await using var output = response.OutputStream;
                await output.WriteAsync(buffer);
                await output.FlushAsync();
            }
            else {
                Console.WriteLine($"Файл {localPath} не обноружен");
                response.StatusCode = (int)HttpStatusCode.NotFound;
                const string errorMessege = "<h2>Ошибка :(</h2><h3>Файл не обноружен</h3>";
                var notFoundBuffer = Encoding.UTF8.GetBytes(errorMessege);
                response.ContentLength64 = notFoundBuffer.Length;
                response.ContentType = "text/html; charset=utf-8";
                await using var output = response.OutputStream;
                await output.WriteAsync(notFoundBuffer);
                await output.FlushAsync();
            }
        } else Successor?.HandleRequest(context);
    }
    public static string GetContentType(string requestUrl) {
        var contentType = Path.GetExtension(requestUrl).ToLower() switch {
            ".svg" => "image/svg+xml",
            ".css" => "text/css",
            ".html" => "text/html; charset=utf-8",
            ".png" => "image/png",
            _ => "text/plain; charset=utf-8"
        };
        return contentType;
    }
    private static void IsDirectoryExistAndCreate(string path) {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
    }
}