using System.Net;
using System.Net.Mail;

namespace CS341_YMCA.Helpers;

/// <summary>
/// Utility for sending basic emails via an SMTP server. Development instances
/// use a throwaway Gmail, and the production noreply is through Private Email.
/// </summary>
public class EmailService
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
    private readonly EmailConfigSection configSection = new();

    public EmailService(IConfiguration Configuration)
    {
        // Bind the SMTP section to the config object
        Configuration.GetSection("SmtpServer").Bind(configSection);
    }

    /// <summary>
    /// Sends an email via a remote SMTP server.
    /// </summary>
    /// <param name="from">Sent email "from" address.</param>
    /// <param name="to">Sent email 'to" address.</param>
    /// <param name="subject">Sent email "subject" line.</param>
    /// <param name="body">Sent email body HTML text.</param>
    public void SendEmail(string? from, string to, string subject, string body)
    {
        // Open an SMTP connection (scoped)
        using var smtp = new SmtpClient(configSection.ServerUrl)
        {
            Port = configSection.Port,
            Credentials = new NetworkCredential(
                configSection.Username,
                configSection.Password),
            EnableSsl = configSection.UseSsl,
        };

        // Create the mail message from the details
        MailMessage mailMessage = new(from ?? configSection.Username, to, subject, body);
        mailMessage.IsBodyHtml = true;
        // Send the constructed message via SMTP
        smtp.Send(mailMessage);
    }
}
