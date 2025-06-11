using AuthService.Errors;
using CSharpFunctionalExtensions;

namespace AuthService.EmailSender;

public interface IMailSender
{
    Task<UnitResult<ErrorList>> SendMailAsync(MailData data);
}