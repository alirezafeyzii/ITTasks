using System.ComponentModel.DataAnnotations.Schema;

namespace ITTasks.DataLayer.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set;}
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string Token { get; set; }

        [ForeignKey("RoleId")]
        public Guid RoleId { get; set; }
        public Role Roles { get; set; }

        public virtual ICollection<ITTask> Tasks { get; set; } = new List<ITTask>();
    }
}
