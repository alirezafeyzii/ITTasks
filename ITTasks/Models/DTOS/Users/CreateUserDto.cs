using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Users
{
    public class CreateUserDto : BaseDTO
    {
        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        public string FullName { get; set; }
		[Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "وارد کردن ایمیل الزامی است")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required(ErrorMessage = "وارد کردن شماره تلفن الزامی است")]
		[DataType(DataType.PhoneNumber, ErrorMessage = "فرمت شماره تلفن درست نیست")]
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "وارد کردن رمز الزامی است")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string Token { get; set; }

	}
}
