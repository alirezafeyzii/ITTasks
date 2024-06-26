﻿using ITTasks.DataLayer.Entities;
using ITTasks.Infrastructure.Helper;
using ITTasks.Models.DTOS.Sprints;
using ITTasks.Models.DTOS.Tasks.TasksType;
using ITTasks.Models.DTOS.Users;
using ITTasks.Models.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ITTasks.Models.DTOS.Tasks
{
    public class ITTaskCreateDto : BaseDTO
	{
        [Required(ErrorMessage = "وارد کردن نام شخص انجام دهنده الزامی است")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "وارد کردن مدت انجام کار الزامی است")]
		public int  Duration { get; set; }
        [Display(Name = "تاریخ")]
        public long Date { get; set; }
        [Required(ErrorMessage = "وارد کردن توضیحات الزامی است")]
        [DataType(DataType.Text)]
        [StringLength(100,ErrorMessage = "توضیحات حداکثر می تواند دارای 100 کاراکتر باشد  ")]
        public string Description { get; set; }
		[Required(ErrorMessage = "وارد کردن واحد الزامی است")]
		public int UnitId { get; set; }
        public string FullName { get; set; }
		public string Title { get; set; }

		public DateTime TaskDateTime { get; set; }
        public List<UserDto> Users { get; set; } 
        public List<ITTaskDto> ITTasks { get; set; }

        public string PersianDate { get;set; }
        public List<ITTaskTypeDto> ITTaskTypes { get; set; }
		[Required(ErrorMessage = "وارد کردن نوع تسک الزامی است")]
		public Guid TaskTypeId { get; set; }
        public bool UserAdded { get; set; } = false;

        public PageInfo pageInfo { get; set; }
        public string PageNumber { get; set; }
        public int Index { get; set; }
		public List<ITTaskDto> ITTasksWithOutPaging { get; set; }
		[Required(ErrorMessage = "وارد کردن اسپرینت الزامی است")]
		public Guid SprintId { get; set; }
        public List<SprintDto> Sprints { get; set; }
        public IFormFile ExcelFile { get; set; }
        public long FromDate { get; set; }
		public long ToDate { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }
	}
}
