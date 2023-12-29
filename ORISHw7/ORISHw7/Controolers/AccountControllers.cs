using MyHttpServer.Attributes;
using MyHttpServer.Configuration;
using MyHttpServer.Services;
namespace MyHttpServer.Controllers;
[Controller("Account")]
public class AccountControllers {
    [Post("EmailSand")]
    public static void SendEmail(string nail, string password) {
        new EmailSenderService(AppSettingsLoader.Instance()?.COnfigure).emailSend(nail, password);
        Console.WriteLine("Email was sent successfully!");
    }
}