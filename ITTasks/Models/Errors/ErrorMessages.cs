﻿namespace ITTasks.Models.Errors
{
	public static class ErrorMessages
	{
		/// <summary>
		/// نوع تاریخ ورودی نادرست است
		/// </summary>
		public static readonly string NotAllowedDateTimeFormat = "نوع تاریخ ورودی نادرست است";

		/// <summary>
		/// بدون خطا
		/// </summary>
		public static readonly string NoError = "بدون خطا";

		/// <summary>
		/// خطای دیتابیس
		/// </summary>
		public static readonly string DatabaseError = "خطای دیتابیس";

		/// <summary>
		/// برخی ورودی ها خالی هستند
		/// </summary>
		public static readonly string NullInputParameters = "برخی ورودی ها خالی هستند";

		/// <summary>
		/// خطای سرور
		/// </summary>
		public static readonly string ServerError = "خطای سرور";

		/// <summary>
		/// نام کاربر نمی تواند خالی باشد
		/// </summary>
		public static readonly string UserFullNameError = "نام کاربر نمی تواند خالی باشد";

		/// <summary>
		/// با موفقیت انجام شد		
		/// </summary>
		public static readonly string SuccessFul = "با موفقیت انجام شد";

		/// <summary>
		/// شناسه کاربر نمی تواند خالی باشد	
		/// </summary>
		public static readonly string UserIdError = "شناسه کاربر نمی تواند خالی باشد";

		/// <summary>
		/// کاربر یافت نشد
		/// </summary>
		public static readonly string UserNotFound = "کاربر یافت نشد";
	}
}
