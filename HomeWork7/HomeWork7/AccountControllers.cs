using Server.Attributes;
using Server.Configuration;
using Server.Services;
namespace MyHttpServer.Controllers;
[Controller("Account")]
public class AccountControllers {
    [Post("SendToEmail")]
    public static void SendToEmail(string email, string password) {
        new EmailSenderService(AppSettingsLoader.Instance()?.Configuration).SendEmail(email, password);
        Console.WriteLine("Email был отправлен");
    }
}