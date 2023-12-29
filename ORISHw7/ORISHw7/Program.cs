using System.Net;
using MyHttpServer;
HttpListener httpListener = new HttpListener();
await new HttpServer(httpListener).Start();