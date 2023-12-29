using System.Net;
using System.Net.Mail;
using Server.AppSetting;
using Server.Services;
namespace Server.Services;
public class EmailSenderService : IEmailSenderService {
    string server;
    string username;
    string password;
    int port;
    public EmailSenderService(AppSettings? config) {
        username = config!.UserName;
        password = config.Password;
        server = config.Server;
        port = config.Port;
    }
    public void EmailSend(string login, string password) {
        var from = new MailAddress(username, "BattleNet");
        Console.WriteLine(11);
        var to = new MailAddress(login);
        Console.WriteLine(12);
        var message = new MailMessage(from, to);
        Console.WriteLine(13);
        var client = new SmtpClient(server);
        Console.WriteLine(14);
        message.Subject = "BattleNet тестовая отправка";
        Console.WriteLine(15);
        message.Body = $"укукукукукукукукукукук";
        Console.WriteLine(16);
        client.Credentials = new NetworkCredential(username, this.password);
        Console.WriteLine(17);
        client.EnableSsl = true;
        Console.WriteLine(18);
        client.Send(message);
        Console.WriteLine(19);
        client.Dispose();
    }
}