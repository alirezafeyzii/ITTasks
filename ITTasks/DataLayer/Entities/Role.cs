using System.ComponentModel.DataAnnotations.Schema;

namespace ITTasks.DataLayer.Entities
{
	public class Role
	{
		public Guid Id { get; set; }
		public string Type { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
		public bool IsActive { get; set; }
		public virtual ICollection<User> Tasks { get; set; } = new List<User>();
	}
}
