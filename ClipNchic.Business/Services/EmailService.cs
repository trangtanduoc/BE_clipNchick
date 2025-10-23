using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ClipNchic.Business.Services;

public class EmailService
{
    private readonly string? _smtpServer;
    private readonly int _smtpPort;
    private readonly string? _senderEmail;
    private readonly string? _senderPassword;
    private readonly string? _appUrl;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["Email:SmtpServer"];
        _smtpPort = int.TryParse(configuration["Email:SmtpPort"], out var port) ? port : 587;
        _senderEmail = configuration["Email:SenderEmail"];
        _senderPassword = configuration["Email:SenderPassword"];
        _appUrl = configuration["Email:AppUrl"];
    }

    public async Task<bool> SendVerificationEmailAsync(string recipientEmail, string verificationToken, string userName = "")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_smtpServer) || string.IsNullOrWhiteSpace(_senderEmail))
            {
                throw new InvalidOperationException("Email configuration is not properly set.");
            }

            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                throw new InvalidOperationException("Recipient email is required.");
            }

            var verificationLink = $"{_appUrl}/verify-email?token={Uri.EscapeDataString(verificationToken)}";
            var subject = "Email Verification - ClipNchic";
            var body = GenerateVerificationEmailBody(userName, verificationLink);
            
            Console.WriteLine($"Attempting to send email to: {recipientEmail}");

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                client.Timeout = 10000;
                
                Console.WriteLine($"SMTP Server: {_smtpServer}:{_smtpPort}");
                Console.WriteLine($"From Email: {_senderEmail}");
                Console.WriteLine($"SSL Enabled: {client.EnableSsl}");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                Console.WriteLine($"Sending email from {_senderEmail} to {recipientEmail}...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"Email sent successfully to {recipientEmail}");
                return true;
            }
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine($"SMTP Error sending email: {smtpEx.Message}");
            Console.WriteLine($"Status Code: {smtpEx.StatusCode}");
            Console.WriteLine($"Inner Exception: {smtpEx.InnerException?.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            Console.WriteLine($"Exception Type: {ex.GetType().Name}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            return false;
        }
    }

    private string GenerateVerificationEmailBody(string userName, string verificationLink)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .button {{ background-color: #4CAF50; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; margin: 20px 0; }}
        .footer {{ color: #666; font-size: 12px; text-align: center; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Email Verification</h2>
        </div>
        <div class='content'>
            <p>Hello {(string.IsNullOrWhiteSpace(userName) ? "User" : userName)},</p>
            <p>Thank you for registering with ClipNchic! To complete your registration, please verify your email address by clicking the button below:</p>
            <a href='{verificationLink}' class='button'>Verify Email</a>
            <p>Or copy this link into your browser:</p>
            <p><small>{verificationLink}</small></p>
            <p>This verification link will expire in 24 hours.</p>
            <p>If you didn't create this account, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 ClipNchic. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
    }
}
