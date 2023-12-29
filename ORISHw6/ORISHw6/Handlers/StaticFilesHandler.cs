using System.Net;
using System.Text;
using System.Web;
using Server.AppSetting;
using Server.Services;
namespace Server.handler;
public class StaticFilesHandler {
    public async Task HandleRequest(HttpListenerContext context) {
        var config = AppSettingsLoader.Instance();
        var response = context.Response;
        var request = context.Request;
        var localPath = request.Url!.LocalPath;
        if (localPath is "" or "/") localPath = "/index.html";
        if (localPath.Contains('.')) {
            var filePath = "../../../" + config.Configuration.StaticPath + localPath;
            if (File.Exists(filePath)) {
                var bytes = await File.ReadAllBytesAsync(filePath);
                response.ContentLength64 = bytes.Length;
                response.ContentType = GetContentType(localPath);
                await using var output = response.OutputStream;
                await output.WriteAsync(bytes);
                await output.FlushAsync();
            } 
        }
        else if (request.HttpMethod == "POST" && request.Url.LocalPath.Equals("/send-email")) {
            Console.WriteLine(1);
            var reader = new StreamReader(request.InputStream);
            Console.WriteLine(2);
            var streamRead = await reader.ReadToEndAsync();
            Console.WriteLine(3);
            var decodedData = HttpUtility.UrlDecode(streamRead, Encoding.UTF8);
            Console.WriteLine(4);
            var str = decodedData.Split("&");
            Console.WriteLine(5);
            var emailSender = new EmailSenderService(config.Configuration);
            Console.WriteLine(6);
            emailSender.EmailSend(str[0].Split("=")[1], str[1].Split("=")[1]);
            Console.WriteLine("Сообщение отправлено успешно");
            Console.WriteLine(7);
            response.RedirectLocation = "/";
            response.StatusCode = (int)HttpStatusCode.Redirect;
            response.Close();
        }
    }
    void ExistDirectory(string path) {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
    }
    static string GetContentType(string requestUrl) {
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
}