using System.Security.Claims;

namespace ITTasks.Models.DTOS.Auth
{
	public class AuthResponse : BaseDTO
	{
		public ClaimsPrincipal Principal { get; set; }
	}
}
