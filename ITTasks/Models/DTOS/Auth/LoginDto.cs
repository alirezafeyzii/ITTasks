using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Auth
{
	public class LoginDto
	{
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نام کاربر را وارد کنید ")]
		public string UserName { get; set; }
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "رمز را وارد کنید ")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
