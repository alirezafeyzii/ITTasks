namespace ITTasks.Statics
{
	public static class UnitsTypes
	{
		enum UnitIds
		{
			
			/// <summary>
			/// اکستنشن
			/// </summary>
			Extention = 1,
			/// <summary>
			/// فروش
			/// </summary>
			Sales = 2,
			/// <summary>
			/// بازرگانی
			/// </summary>
			Commerce = 3,
			/// <summary>
			/// منابع انسانی
			/// </summary>
			HR = 4,
			/// <summary>
			/// تحقیق و توسعه
			/// </summary>
			RAndD = 5,
			/// <summary>
			/// زیما
			/// </summary>
			Xima = 6,
			/// <summary>
			/// خدمات
			/// </summary>
			Services = 7,
			/// <summary>
			/// فناوری اطلاعات
			/// </summary>
			IT = 8
		}

		public static readonly Dictionary<int, string> keyValues = new Dictionary<int, string>()
		{
			{ (int)UnitIds.IT,"فناوری اطلاعات"},
			{ (int)UnitIds.Extention,"اکستنشن"},
			{ (int)UnitIds.Sales,"فروش"},
			{ (int)UnitIds.Commerce,"بازرگانی"},
			{ (int)UnitIds.HR,"منابع انسانی"},
			{ (int)UnitIds.RAndD,"تحقیق و توسعه"},
			{ (int)UnitIds.Xima,"زیما"},
			{ (int)UnitIds.Services,"خدمات"},
		};

		public static string GetUnitName(this int id)
		{
			return keyValues[id] ?? "";
		}
	}
}
