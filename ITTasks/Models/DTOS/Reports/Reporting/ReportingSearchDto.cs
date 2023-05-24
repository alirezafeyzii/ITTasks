﻿namespace ITTasks.Models.DTOS.Reports.Reporting
{
	public class ReportingSearchDto
	{
		public List<string> SprintIds { 
			get {
				return _sprintIds.Select(s => s.ToString()).ToList();
			}
			set {
				_sprintIds = value.Select(val => Guid.Parse(val)).ToList();
				//SprintIds = value;
			}
		}
		List<Guid>? _sprintIds { get; set; } = new List<Guid>();


		public List<string> UserIds
		{
			get
			{
				return _userIds.Select(s => s.ToString()).ToList();
			}
			set
			{
				_userIds = value.Select(val => Guid.Parse(val)).ToList();
				//SprintIds = value;
			}
		}
		List<Guid>? _userIds { get; set; } = new List<Guid>();
	}
}
