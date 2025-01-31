using Microsoft.AspNetCore.Identity.UI.Services;
namespace EcommerceBookApp.Utility
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			//TODO: Need to implement the feature to send emai;
			return Task.CompletedTask;
		}
	}
}
