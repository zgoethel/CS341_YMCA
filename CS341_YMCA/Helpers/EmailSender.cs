using System.Net;
using System.Net.Mail;

namespace CS341_YMCA.Helpers;

/// <summary>
/// Utility for sending basic emails via an SMTP server.
/// </summary>
public class EmailSender
{
    /// <summary>
    /// Email configuration section field definitions.
    /// </summary>
    private class EmailConfigSection
    {
        public string ServerUrl { get; set; } = "";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseSsl { get; set; } = true;
    }

    /// <summary>
    /// Object bound to the email config section.
    /// </summary>
    private readonly EmailConfigSection ConfigSection = new();

    public EmailSender(IConfiguration Configuration)
    {
        // Bind the SMTP section to the config object
        Configuration.GetSection("SmtpServer").Bind(ConfigSection);
    }

    /// <summary>
    /// Sends an email via a remote SMTP server.
    /// </summary>
    /// <param name="From">Sent email "from" address.</param>
    /// <param name="To">Sent email 'to" address.</param>
    /// <param name="Subject">Sent email "subject" line.</param>
    /// <param name="Body">Sent email body HTML text.</param>
    public void SendEmail(string? From, string To, string Subject, string Body)
    {
        // Open an SMTP connection (scoped)
        using var smtp = new SmtpClient(ConfigSection.ServerUrl)
        {
            Port = ConfigSection.Port,
            Credentials = new NetworkCredential(
                ConfigSection.Username,
                ConfigSection.Password),
            EnableSsl = ConfigSection.UseSsl,
        };

        // Create the mail message from the details
        MailMessage mailMessage = new(From ?? ConfigSection.Username, To, Subject, Body);
        mailMessage.IsBodyHtml = true;
        // Send the constructed message via SMTP
        smtp.Send(mailMessage);
    }
}
