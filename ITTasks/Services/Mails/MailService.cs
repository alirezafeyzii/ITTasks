using ITTasks.Models.DTOS.Emails;
using ITTasks.Settings.Mail;
using ITTasks.Statics;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ITTasks.Services.Mails
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;
		public MailService(IOptions<MailSettings> mailSettings)
		{
			_mailSettings = mailSettings.Value;
		}

		public async Task<bool> SendMailForConfirmedAsync(MailRequestDto request)
		{
			string baseUrlServer = $"{Urls.BaseUrl}/Auth/ConfirmEmail";
			string baseUrlLocal = "https://localhost:7180/Auth/ConfirmEmail";
			try
			{
				MimeMessage mail = new MimeMessage();
				mail.From.Add(new MailboxAddress("ITTasks ", "ittasks@mehbang.group"));
				mail.To.Add(new MailboxAddress(request.UserName, request.ToEmail));
				mail.Subject = request.Subject;
				mail.Body = new TextPart(TextFormat.Html) { Text = $"<h3>{request.UserFullName} عزیز برای فعالسازی حساب کاربری خود دکمه زیر را بفشارید </h3><a href=\"{baseUrlServer}?userName={request.UserName}&token={request.Token}\"><button style=\"padding:10px;background-color:#6475f5;color:white;cursor: pointer;border-radius:5px;outline:none;border:none;\">فعالسازی</button></a>" };

				SmtpClient smtp = new SmtpClient();

				await smtp.ConnectAsync("mail.mehbang.group", 25, SecureSocketOptions.None);
				await smtp.AuthenticateAsync("ittasks@mehbang.group", "Datis@123");
				await smtp.SendAsync(mail);
				await smtp.DisconnectAsync(true);
				return true;


			}
			catch (Exception e)
			{
				return false;
			}

		}

		public async Task<bool> SendMailForgetPasswordAsync(MailRequestDto request)
		{
			try
			{
				MimeMessage mail = new MimeMessage();
				mail.From.Add(new MailboxAddress("ITTasks ", "ittasks@mehbang.group"));
				mail.To.Add(new MailboxAddress(request.UserFullName, request.ToEmail));
				mail.Subject = request.Subject;
				mail.Body = new TextPart(TextFormat.Html) { Text = $"<h3 style=\"direction:rtl;\" >{request.UserFullName} عزیز رمز عبور شما :</h3><pre>{request.Data}</pre>" };

				SmtpClient smtp = new SmtpClient();

				await smtp.ConnectAsync("mail.mehbang.group", 25, SecureSocketOptions.None);
				await smtp.AuthenticateAsync("ittasks@mehbang.group", "Datis@123");
				await smtp.SendAsync(mail);
				await smtp.DisconnectAsync(true);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
	}
}
