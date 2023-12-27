using System.Net;
using System.Net.Mail;
using Server.Configuration;
using Server.services;

namespace Server.Services;
public class EmailSenderService : IEmailSenderService
{
    private readonly string Server;
    private readonly string UserName;
    private readonly string Password;
    private readonly ushort Port;

    public EmailSenderService(AppSettings? config) {
        UserName = config!.Username;
        Password = config.Password;
        Server = config.Server;
        Port = config.Port;
    }
    public void SendEmail(string login, string password) {
        var from = new MailAddress(UserName, "BattleNet");
        var to = new MailAddress(login);
        var message = new MailMessage(from, to);
        message.Subject = "BattleNet данные логина";
        message.Body = $"Логин: {login}\nПароль: {password}";
        var smtpClient = new SmtpClient(Server);
        smtpClient.Credentials = new NetworkCredential(UserName, Password);
        smtpClient.EnableSsl = true;
        smtpClient.Send(message);
        smtpClient.Dispose();
    }
}