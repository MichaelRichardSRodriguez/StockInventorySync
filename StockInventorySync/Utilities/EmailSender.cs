using Microsoft.AspNetCore.Identity.UI.Services;

namespace StockInventorySync.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Insert Logic for sending email
            return Task.CompletedTask;
        }
    }
}
