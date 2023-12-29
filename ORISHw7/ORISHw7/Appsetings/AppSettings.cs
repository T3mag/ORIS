namespace MyHttpServer.Configuration;

public class AppSettings
{
    public int Port { get; }
    public string Address { get; }
    public string StaticPath { get; }
    public string Username { get; }
    public string Password { get; }
    public string Server { get;}
    public int MailPort { get; }
    public AppSettings(int port = 0, string address = "", string staticPath = "", string username = "", string password = "", string server = "", int mailPort = 0) {
        Port = port; Address = address; StaticPath = staticPath; Username = username; Password = password; Server = server; MailPort = mailPort;
    }
}