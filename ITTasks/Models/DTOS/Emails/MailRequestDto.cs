namespace ITTasks.Models.DTOS.Emails
{
	public class MailRequestDto
	{
        public string ToEmail { get; set; }
		public string UserName { get; set; }
		public string Subject { get; set; }
        public string Body { get; set; }
        public string  UserFullName { get; set; }
		public string Token { get; set; }
		public string Data { get; set; }
	}
}
