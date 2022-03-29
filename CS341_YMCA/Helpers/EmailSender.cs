using System.Net;
using System.Net.Mail;

namespace CS341_YMCA.Helpers;

public class EmailSender
{
    private class EmailConfigSection
    {
        public string ServerUrl { get; set; } = "";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseSsl { get; set; } = true;
    }

    private readonly EmailConfigSection ConfigSection = new();

    public EmailSender(IConfiguration Configuration)
    {
        Configuration.GetSection("SmtpServer").Bind(ConfigSection);
    }

    public void SendEmail(string? From, string To, string Subject, string Body)
    {
        using var Smtp = new SmtpClient(ConfigSection.ServerUrl)
        {
            Port = ConfigSection.Port,
            Credentials = new NetworkCredential(
                ConfigSection.Username,
                ConfigSection.Password),
            EnableSsl = ConfigSection.UseSsl,
        };

        MailMessage MailMessage = new(From ?? ConfigSection.Username, To, Subject, Body);
        MailMessage.IsBodyHtml = true;

        Smtp.Send(MailMessage);
    }
}
