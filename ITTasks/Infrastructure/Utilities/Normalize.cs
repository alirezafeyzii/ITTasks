namespace ITTasks.Infrastructure.Utilities
{
	public static class Normalize
	{
		public static string ToNormalize(this string input)
		{
			try
			{
				return input.Trim().ToUpper().Replace(" ", "");
			}
			catch (Exception)
			{

				return string.Empty;
			}
		}
	}
}
