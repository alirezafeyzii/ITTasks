using ITTasks.Models.DTOS.Auth;
using ITTasks.Models.Errors;
using ITTasks.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ITTasks.Services.Auth
{
	public class AuthService : IAuthService
	{
		private readonly IUserService _userService;

		public AuthService(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<AuthResponse> SignInAsync(LoginDto request)
		{
			try
			{
				if (request is null)
				{
					return new AuthResponse
					{
						ErrorCode = (int)ErrorCodes.NullObjectError,
						ErrorMessage = ErrorMessages.NullInputParameters
					};
				}

				if (request.UserName is null)
				{
					return new AuthResponse
					{
						ErrorCode = (int)ErrorCodes.UserNameError,
						ErrorMessage = ErrorMessages.PasswordError
					};
				}

				var userFromService = await _userService.GetUserForSignInAsync(request.UserName, request.Password);
				if (userFromService.ErrorCode != (int)ErrorCodes.NoError)
				{
					return new AuthResponse
					{
						ErrorCode = userFromService.ErrorCode,
						ErrorMessage = userFromService.ErrorMessage
					};
				}

				//request.RememberMe == true ? expirationTime = $"{DateTimeOffset.UtcNow.AddHours(48).ToUnixTimeSeconds()}" : expirationTime = $"{DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds()}";

				var claims = new List<Claim>
				{
						new Claim(ClaimTypes.NameIdentifier, userFromService.Id.ToString()),
						new Claim(ClaimTypes.Name, userFromService.FullName),
						new Claim(ClaimTypes.Role, userFromService.Role.Type),
				};
				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);


				return new AuthResponse
				{
					ErrorCode = (int)ErrorCodes.NoError,
					ErrorMessage = ErrorMessages.NoError,
					Principal = principal
				};
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
