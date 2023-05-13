namespace ITTasks.Infrastructure.Utilities
{
	public static class DateTimeUtility
	{
		public static DateTime UnixToDateTime(this long unix)
		{
			try
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unix).ToLocalTime();
			}
			catch (Exception)
			{
				return new DateTime(1, 1, 1);
			}
		}
	}
}
