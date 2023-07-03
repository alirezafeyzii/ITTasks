using ITTasks.Models.DTOS.Auth;
using ITTasks.Models.DTOS.Users;
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

		public async Task<AuthResponse> EmailConfirmedAsync(string userName, string token)
		{
			var userFromService = await _userService.ConfirmEmailAsync(userName, token);
			if(userFromService.ErrorCode != (int)ErrorCodes.NoError)
			{
				return new AuthResponse
				{
					ErrorCode = userFromService.ErrorCode,
					ErrorMessage = userFromService.ErrorMessage
				};
			}

			var principal = SetClaim(userFromService.Id.ToString(), userFromService.FullName, userFromService.Role.Type);

			return new AuthResponse
			{
				ErrorCode = (int)ErrorCodes.NoError,
				ErrorMessage = ErrorMessages.NoError,
				Principal = principal
			};
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
						ErrorMessage = ErrorMessages.UserNameError
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

				var principal = SetClaim(userFromService.Id.ToString(), userFromService.FullName, userFromService.Role.Type);


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

		public async Task<AuthResponse> SignUpAsync(CreateUserDto request)
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

				if (request.FullName is null)
				{
					return new AuthResponse
					{
						ErrorCode = (int)ErrorCodes.UserFullNameError,
						ErrorMessage = ErrorMessages.UserFullNameError
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

				if (request.Email is null)
				{
					return new AuthResponse
					{
						ErrorCode = (int)ErrorCodes.EmailError,
						ErrorMessage = ErrorMessages.EmailError
					};
				}

				if (request.Password is null)
				{
					return new AuthResponse
					{
						ErrorCode = (int)ErrorCodes.PasswordError,
						ErrorMessage = ErrorMessages.PasswordError
					};
				}

				if (request.PhoneNumber is null)
				{
					return new AuthResponse
					{
						ErrorCode = (int)ErrorCodes.PhoneNumber,
						ErrorMessage = ErrorMessages.PhoneNumberError
					};
				}

				var userFromService = await _userService.CreateUserAsync(request);
				if(userFromService.ErrorCode != (int)ErrorCodes.NoError)
				{
					return new AuthResponse
					{
						ErrorCode = userFromService.ErrorCode,
						ErrorMessage = userFromService.ErrorMessage
					};
				}
				var principal = SetClaim(userFromService.Id.ToString(), userFromService.FullName, userFromService.Role.Type);

				return new AuthResponse
				{
					ErrorCode = (int)ErrorCodes.NoError, 
					ErrorMessage = ErrorMessages.NoError,
					Principal = principal,
					UserEmail = userFromService.Email,
					UserToken = userFromService.Token,
					UserFullName = userFromService.FullName,
					UserName = userFromService.UserName
				};
			}
			catch (Exception)
			{
				throw;
			}
		}

		private ClaimsPrincipal SetClaim(string id, string name, string roleType)
		{
			var claims = new List<Claim>
				{
						new Claim(ClaimTypes.NameIdentifier, id),
						new Claim(ClaimTypes.Name, name),
						new Claim(ClaimTypes.Role, roleType)
				};
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);
			return principal;
		}
	}
}
