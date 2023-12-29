using System.Net;
using System.Reflection;
using System.Text;
using MyHttpServer.Attributes;
using Newtonsoft.Json;
namespace MyHttpServer.Handlers;
public class ControllersHandler: Handler {
    public override void HandleRequest(HttpListenerContext context) {
        try {
            var request = context.Request;
            var response = context.Response;
            var parameters = request.Url!.Segments.Skip(1).Select(s => s.Replace("/", "")).ToArray();
            var tempOfData = new StreamReader(context.Request.InputStream).ReadToEnd();
            Console.WriteLine(tempOfData);
            var formData = new[] { "" };
            if (!string.IsNullOrEmpty(tempOfData)) {
                var currentOfUserData = tempOfData.Split('&');
                formData = new[] { WebUtility.UrlDecode(currentOfUserData[0][6..]), currentOfUserData[1][9..] };
            }
            var controllerName = parameters[0];
            var methodName = parameters[1];
            var assembly = Assembly.GetExecutingAssembly();
            var controller = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(ControllerAttribute))).FirstOrDefault(c =>
                    ((ControllerAttribute)Attribute.GetCustomAttribute(c, typeof(ControllerAttribute))!).ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
            var method = (controller?.GetMethods()!).FirstOrDefault(x => x.GetCustomAttributes(true).Any(attr => attr.GetType().Name.Equals($"{request.HttpMethod}Attribute", 
                    StringComparison.OrdinalIgnoreCase) && ((HttpMethodAttribute)attr).ActionName.Equals(methodName, StringComparison.OrdinalIgnoreCase)));
            var queryParams = Array.Empty<object>();
            if (formData.Length > 1) {
                queryParams = method?.GetParameters().Select((p, i) => Convert.ChangeType(formData[i], p.ParameterType)).ToArray();
            }
            var resultFromMethod = method?.Invoke(Activator.CreateInstance(controller!), queryParams);
            if (!(method!.ReturnType == typeof(void))) ProcessResult(resultFromMethod, response, context);
            else response.Redirect("http://127.0.0.1:1414/");
        } catch (ArgumentNullException exception) {
            Console.WriteLine(exception.Message);
        }
    }
    private static void ProcessResult<T>(T res, HttpListenerResponse response, HttpListenerContext context) {
        switch (res) {
            case string resultOfString: {
                response.ContentType = StaticFilesHandler.GetContentType(context.Request.Url!.LocalPath);
                var bytes = Encoding.UTF8.GetBytes(resultOfString);
                response.ContentLength64 = bytes.Length;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                break;
            } case T[] arrayOfObjects: {
                response.ContentType = StaticFilesHandler.GetContentType(context.Request.Url!.LocalPath);
                var json = JsonConvert.SerializeObject(arrayOfObjects, Formatting.Indented);
                var bytes = Encoding.UTF8.GetBytes(json);
                response.ContentLength64 = bytes.Length;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                break;
            }
        }
    }
}