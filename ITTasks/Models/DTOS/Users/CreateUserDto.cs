using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Users
{
    public class CreateUserDto : BaseDTO
    {
        [Required(ErrorMessage = "وارد کردن نام کاربر الزامی است")]
        public string FullName { get; set; }
    }
}
