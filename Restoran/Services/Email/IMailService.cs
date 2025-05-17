using Restoran.Helper.Email;

namespace Restoran.Services.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
