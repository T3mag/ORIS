using System.Net;
using System.Text;
using MyHttpServer.Configuration;

namespace MyHttpServer.Handlers;

public class StaticFilesHandler: Handler {
    public override async void HandleRequest(HttpListenerContext context) {
        var config = AppSettingsLoader.Instance();
        var response = context.Response;
        var request = context.Request;
        var localPath = request.Url!.LocalPath;
        ExistDirectory("../../../" + config!.COnfigure!.StaticPath);
        if (localPath is "" or "/") localPath = "/index.html";
        if (localPath.Contains('.')) {
            var filePath = "../../../" + config.COnfigure.StaticPath + localPath;
            if (File.Exists(filePath)) {
                var bytes = await File.ReadAllBytesAsync(filePath);
                response.ContentLength64 = bytes.Length;
                response.ContentType = GetContentType(localPath);
                await using var output = response.OutputStream;
                await output.WriteAsync(bytes);
                await output.FlushAsync();
            } else {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                const string errorHtml = "<h2>Ошибка подключения</h2>";
                response.ContentLength64 = Encoding.UTF8.GetBytes(errorHtml).Length;
                await using var output = response.OutputStream;
                await output.WriteAsync(Encoding.UTF8.GetBytes(errorHtml));
                await output.FlushAsync();
            }
        } else Incomer?.HandleRequest(context);
    }
    public static string GetContentType(string requestUrl) {
        var contentType = Path.GetExtension(requestUrl).ToLower() 
            switch { ".html" => "text/html; charset=utf-8",
            ".png" => "image/png",
            ".css" => "text/css",
            ".svg" => "image/svg+xml",
            "webp" => "text/webp",
            _ => throw new ArgumentOutOfRangeException()
            };
        return contentType;
    }
    private static void ExistDirectory (string path) {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
    }
}