using System.Net;
using System.Net.Mail;
using MyHttpServer.Configuration;
using MyHttpServer.services;
namespace MyHttpServer.Services;
public class EmailSenderService : IEmailSenderService {
    string Server;
    string Username;
    string Password;
    int Port;
    public EmailSenderService(AppSettings? config) {
        Username = config!.Username;
        Password = config.Password;
        Server = config.Server;
        Port = config.Port;
    }
    public void emailSend(string login, string password) {
        var message = new MailMessage(new MailAddress(Username, "BattleNet"), new MailAddress(login));
        message.Subject = "Тест";
        message.Body = $"тест";
        var smtpClient = new SmtpClient(Server);
        smtpClient.Credentials = new NetworkCredential(Username, Password);
        smtpClient.EnableSsl = true;
        smtpClient.Send(message);
        smtpClient.Dispose();
    }
}