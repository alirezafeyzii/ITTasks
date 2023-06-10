namespace ITTasks.Models.Errors
{
	public static class ExcelErrors
	{
		public static class ExcelErrorMessages
		{
			/// <summary>
			/// فرمت فایل وارد شده اکسل نمی باشد
			/// </summary>
			public static readonly string ImportedFileIsNotExcel = "فرمت فایل وارد شده اکسل نمی باشد";

			/// <summary>
			/// تسکی برای اضافه کردن یافت نشد لطفا تسک را به درستی در اکسل وارد کنید
			/// </summary>
			public static readonly string TasksWasNotFound = "تسکی برای اضافه کردن یافت نشد لطفا تسک را به درستی در اکسل وارد کنید";

			/// <summary>
			/// فرمت پارامتر تاریخ را به درستی وارد نکرده اید
			/// </summary>
			public static readonly string DateFormatParameters = "فرمت پارامتر تاریخ را به درستی وارد نکرده اید";

			/// <summary>
			/// فرمت پارامتر مدت را به درستی وارد نکرده اید
			/// </summary>
			public static readonly string DurationFormatParameters = "فرمت پارامتر مدت را به درستی وارد نکرده اید";

			/// <summary>
			/// واحد یافت نشد
			/// </summary>
			public static readonly string UnitWasNotFound = "واحد یافت نشد";

			/// <summary>
			/// خطای سرور
			/// </summary>
			public static readonly string ServerError = "خطای سرور";

			/// <summary>
			/// بدون خطا
			/// </summary>
			public static readonly string NoError = "بدون خطا";

		}

		public static class ExcelErrorHandling
		{
			/// <summary>
			/// خطای کاربر
			/// </summary>
			public const string UserError = "UserError";

			/// <summary>
			/// خطای اسپرینت
			/// </summary>
			public const string SprintError = "SprintError";

			/// <summary>
			/// خطای نوع تسک
			/// </summary>
			public const string TaskTypeError = "TaskTypeError";

			/// <summary>
			/// خطای واحد
			/// </summary>
			public const string UnitError = "UnitError";

			/// <summary>
			/// خطای فرمت تاریخ
			/// </summary>
			public const string DateFormatError = "DateFormatError";

			/// <summary>
			/// خطای فرمت مدت
			/// </summary>
			public const string DurationFormatError = "DurationFormatError";


			public static string ExcelErrorParams(string type, string value, int rowIndex)
			{
				string message = "";
				switch (type)
				{
					case UserError:
						message += $"کاربر مورد نظر از ردیف  {rowIndex} یافت نشد.";
						break;
					case SprintError:
						message += $"اسپرینت مورد نظر از ردیف  {rowIndex} یافت نشد.";
						break;
					case TaskTypeError:
						message += $"نوع تسک مورد نظر از ردیف  {rowIndex} یافت نشد.";
						break;
					case UnitError:
						message += $"واحد مورد نظر از ردیف  {rowIndex} یافت نشد.";
						break;
					case DateFormatError:
						message += $"فرمت تاریخ وارد شده از ردیف {rowIndex}  به درستی وارد نشده.";
						break;
					case DurationFormatError:
						message += $"فرمت مدت وارد شده از ردیف {rowIndex}  به درستی وارد نشده.";
						break;
					default:
						message += $"خطایی در سطر {rowIndex} رخ داده است.";
						break;
				}

				message += $"مقدار ورودی : {value}";

				return message;
			}

		}
	}
}
