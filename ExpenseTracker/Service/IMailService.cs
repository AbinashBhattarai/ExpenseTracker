using ExpenseTracker.ViewModel;

namespace ExpenseTracker.Service
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
