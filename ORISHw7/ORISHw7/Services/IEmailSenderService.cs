namespace MyHttpServer.services;
public interface IEmailSenderService {
    public void emailSend(string login, string password);
}