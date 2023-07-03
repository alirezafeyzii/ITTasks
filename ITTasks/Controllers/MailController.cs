using ITTasks.Models.DTOS.Emails;
using ITTasks.Services.Mails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITTasks.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MailController : ControllerBase
	{
		private readonly IMailService mailService;
		public MailController(IMailService mailService)
		{
			this.mailService = mailService;
		}
		[HttpPost("send")]
		public async Task<IActionResult> SendMail([FromForm] MailRequestDto request)
		{
			try
			{
				await mailService.SendMailForConfirmedAsync(request);
				return Ok();
			}
			catch (Exception ex)
			{
				throw;
			}

		}
	}
}
