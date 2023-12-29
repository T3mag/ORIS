using System.Net;
using Server;

var httpLisener = new HttpListener();
await new HttpServer(httpLisener).Start();