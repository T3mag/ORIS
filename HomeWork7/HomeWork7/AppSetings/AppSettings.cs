namespace Server.Configuration;

public class AppSettings {
    public ushort Port { get; private set; }
    public string Address { get; private set; }
    public string StaticFilesPath { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Server { get; private set; }
    public ushort SmtpPort { get; private set; }
    public AppSettings(ushort port = 0, string address = "", string staticFilesPath = "",
        string username = "", string password = "", string server = "", ushort smtpPort = 0) {
        Port = port;
        Address = address;
        StaticFilesPath = staticFilesPath;
        Username = username;
        Password = password;
        Server = server;
        SmtpPort = smtpPort;
    }
}