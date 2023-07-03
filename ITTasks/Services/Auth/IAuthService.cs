using ITTasks.Models.DTOS.Auth;
using ITTasks.Models.DTOS.Users;

namespace ITTasks.Services.Auth
{
	public interface IAuthService
	{
		public Task<AuthResponse> SignInAsync(LoginDto request);
		public Task<AuthResponse> SignUpAsync(CreateUserDto request);
		public Task<AuthResponse> EmailConfirmedAsync(string userName, string token);		
	}
}
