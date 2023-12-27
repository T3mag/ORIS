using System.Net;
using System.Net.Mail;
using MyHttpServer.Configuration;
using MyHttpServer.services;
namespace MyHttpServer.Services;
public class EmailSenderService : IEmailSenderService {
    private readonly string server;
    private readonly string username;
    private readonly string password;
    private readonly ushort port;
    public EmailSenderService(AppSettings? config) {
        server = config.SmtpServer;
        port = config.Port;
        username = config!.SmtpUsername;
        password = config.SmtpPassword;
    }
    public void SendEmail(string login, string password) {
        var from = new MailAddress(username, "BattleNet");
        var to = new MailAddress(login);
        var message = new MailMessage(from, to);
        message.Subject = "BattleNet Login Details";
        message.Body = $"Login: {login}\nPassword: {WebUtility.HtmlDecode(password)}";
        var smtpClient = new SmtpClient(server);
        smtpClient.Credentials = new NetworkCredential(username, this.password);
        smtpClient.EnableSsl = true;
        smtpClient.Send(message);
        smtpClient.Dispose();
    }
}