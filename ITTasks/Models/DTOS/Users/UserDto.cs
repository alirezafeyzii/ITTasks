using ITTasks.DataLayer.Entities;

namespace ITTasks.Models.DTOS.Users
{
    public class UserDto : BaseDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Password { get; set; }
        public string Token { get; set; }
		public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public List<UserDto> Users { get; set; }
        public Role Role { get; set; }

        public static implicit operator UserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                IsActive = user.IsActive,
                CreatedTime = user.CreatedTime,
                UpdatedTime = user.UpdatedTime,
            };
        }
    }
}
