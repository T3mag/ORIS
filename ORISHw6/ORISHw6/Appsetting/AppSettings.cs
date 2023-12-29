namespace Server.AppSetting;
public class AppSettings {
    public int Port { get; }
    public string Address { get; }
    public string StaticPath { get; }
    public string UserName { get; }
    public string Password { get; }
    public string Server { get; }
    public int SmtpPort { get; }
    public AppSettings( int port = 0, string address = "", string staticPath = "", string userName = "", string password = "", string server = "", int smtpPort = 0) {
        Port = port; Address = address; UserName = userName; Password = password; Server = server; SmtpPort = smtpPort; StaticPath = staticPath;
    }
}