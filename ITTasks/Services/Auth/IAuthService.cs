using ITTasks.Models.DTOS.Auth;

namespace ITTasks.Services.Auth
{
	public interface IAuthService
	{
		public Task<AuthResponse> SignInAsync(LoginDto request);
	}
}
