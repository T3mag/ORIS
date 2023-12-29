namespace Server.Services;

public interface IEmailSenderService {
    public void EmailSend(string login, string password);
}