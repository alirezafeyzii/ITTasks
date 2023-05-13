using System.ComponentModel.DataAnnotations;

namespace ITTasks.Models.DTOS.Tasks.TasksType
{
	public class ITTaskTypeCreateDto : BaseDTO
	{
		[Required(ErrorMessage = "وارد کردن موضوع الزامی است")]
		public string Title { get; set; }
	}
}
