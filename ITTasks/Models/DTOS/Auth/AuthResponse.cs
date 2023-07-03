using System.Security.Claims;

namespace ITTasks.Models.DTOS.Auth
{
	public class AuthResponse : BaseDTO
	{
		public ClaimsPrincipal Principal { get; set; }
		public string UserEmail { get; set; }
		public  string UserToken { get; set; }
		public string UserFullName { get; set; }
        public string UserName { get; set; }
    }
}
