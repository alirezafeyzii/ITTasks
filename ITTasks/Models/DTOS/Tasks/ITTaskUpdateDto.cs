using ITTasks.Models.Errors;
using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Tasks
{
	public class ITTaskUpdateDto
	{
		[Required(ErrorMessage = "وارد کردن نام شخص انجام دهنده الزامی است")]
		public Guid Id { get; set; }
		[Required(ErrorMessage = "وارد کردن اسپرینت الزامی است")]
		public Guid SprintId { get; set; }
		[Required(ErrorMessage = "وارد کردن نوع تسک الزامی است")]
		public Guid TaskTypeId { get; set; }
		[Required(ErrorMessage = "وارد کردن نام کاربر الزامی است")]
		public Guid UserId { get; set; }
		[Required(ErrorMessage = "وارد کردن مقدار ساعت کار الزامی است")]
		public float HourAmount { get; set; }
		[Required(ErrorMessage = "وارد کردن توضیحات الزامی است")]
		[DataType(DataType.Text)]
		[StringLength(100, ErrorMessage = "توضیحات حداکثر می تواند دارای 100 کاراکتر باشد  ")]
		public string Description { get; set; }
		public long Date { get; set; }
	}
}
