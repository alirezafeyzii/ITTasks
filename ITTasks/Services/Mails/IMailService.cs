using ITTasks.Models.DTOS.Emails;

namespace ITTasks.Services.Mails
{
	public interface IMailService
	{
		public Task<bool> SendMailForConfirmedAsync(MailRequestDto request);
		public Task<bool> SendMailForgetPasswordAsync(MailRequestDto request);
	}
}
