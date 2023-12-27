using System.Net;
namespace Server.Handlers;
public abstract class Handler {
    public Handler? Successor { get; set; }
    public abstract void HandleRequest(HttpListenerContext context);
}