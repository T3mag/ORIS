using System.Net;
namespace MyHttpServer.Handlers;
public abstract class Handler {
    public abstract void HandleRequest(HttpListenerContext context);
    public Handler? Incomer { get; set; }
}