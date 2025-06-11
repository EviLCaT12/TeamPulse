using AuthService.Errors;
using CSharpFunctionalExtensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AuthService.EmailSender;

public class EmailSender : IMailSender
{
    private readonly MailOptions _mailOptions;

    public EmailSender(IOptions<MailOptions> options)
    {
        _mailOptions = options.Value;
    }

    public async Task<UnitResult<ErrorList>> SendMailAsync(MailData data)
    {
        var mail = new MimeMessage();

        mail.From.Add(new MailboxAddress(_mailOptions.FromDisplayName, _mailOptions.From));

        var tryParse = MailboxAddress.TryParse(data.To, out var address);
        if (tryParse == false)
        {
            return Errors.Errors.General.
                ValueIsInvalid($"{data.To} is not a valid email address.")
                .ToErrorList();
        }
        
        mail.To.Add(address);

        var body = new BodyBuilder
        {
            HtmlBody = data.Body
        };

        mail.Body = body.ToMessageBody();
        mail.Subject = data.Subject;

        using var client = new SmtpClient();
        
        await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port);
        await client.AuthenticateAsync(_mailOptions.Username, _mailOptions.Password);
        await client.SendAsync(mail);

        return UnitResult.Success<ErrorList>();
    }
}