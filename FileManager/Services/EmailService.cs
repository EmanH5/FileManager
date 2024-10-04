using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration configuration)
    {
        _apiKey = configuration["SendGrid:ApiKey"];
        _senderEmail = configuration["SendGrid:SenderEmail"];
        _senderName = configuration["SendGrid:SenderName"];
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_senderEmail, _senderName);
        var to = new EmailAddress(recipientEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
        await client.SendEmailAsync(msg);
    }
}
