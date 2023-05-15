using ITTasks.Models.Errors;
using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Units
{
	public class UpdateUnitDto
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "وارد کردن نام واحد الزامی است")]
		public string Name { get; set; }
	}
}
