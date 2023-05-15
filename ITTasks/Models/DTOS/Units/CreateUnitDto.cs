using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Units
{
	public class CreateUnitDto
	{
		[Required(ErrorMessage = "وارد کردن نام واحد الزامی است")]
		public string Name { get; set; }
	}
}
