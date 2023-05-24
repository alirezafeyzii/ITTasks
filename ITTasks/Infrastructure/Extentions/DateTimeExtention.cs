using System.Globalization;

namespace ITTasks.Infrastructure.Extentions
{
    public static class DateTimeExtention
    {
        public static string ToPersian(this DateTime dateTime, string? format = default)
        {
            PersianCalendar persianCalendar = new();
            string year = persianCalendar.GetYear(dateTime).ToString();
            string month = persianCalendar.GetMonth(dateTime).ToString().PadLeft(2, '0');
            string day = persianCalendar.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0');
            string hour = dateTime.Hour.ToString().PadLeft(2, '0');
            string minute = dateTime.Minute.ToString().PadLeft(2, '0');
            string second = dateTime.Second.ToString().PadLeft(2, '0');

            if (format is null)
            {
                return $"{year}/{month}/{day} {hour}:{minute}:{second}";
            }

            format = format.Replace("yyyy", year);
            format = format.Replace("MM", month);
            format = format.Replace("dd", day);
            format = format.Replace("hh", hour);
            format = format.Replace("mm", minute);
            format = format.Replace("ss", second);

            return format;
        }

		public static string ToPersianWithOutTime(this DateTime dateTime, string? format = default)
		{
			PersianCalendar persianCalendar = new();
			string year = persianCalendar.GetYear(dateTime).ToString();
			string month = persianCalendar.GetMonth(dateTime).ToString().PadLeft(2, '0');
			string day = persianCalendar.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0');

			if (format is null)
			{
				return $"{year}/{month}/{day} ";
			}

			format = format.Replace("yyyy", year);
			format = format.Replace("MM", month);
			format = format.Replace("dd", day);

			return format;
		}

        public static string ToStandardTime(this int time)
        {
            try
            {
				var hour = time / 60;
				var minutes = time % 60;

				return $"{hour.ToString("00")}:{minutes.ToString("00")}";
			}
            catch (Exception)
            {

                return "-00:-00";
            }

        }
	}
}
