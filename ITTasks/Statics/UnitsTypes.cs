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
			IT = 8,
			/// <summary>
			/// سایر
			/// </summary>
			Other = 9,
			/// <summary>
			/// کارخانه
			/// </summary>
			Factory = 10,
			/// <summary>
			/// حسابداری
			/// </summary>
			Accounting = 11,
			/// <summary>
			/// گرافیک
			/// </summary>
			Graphics = 12,
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
			{ (int)UnitIds.Other,"سایر"},
			{ (int)UnitIds.Factory,"کارخانه"},
			{ (int)UnitIds.Accounting,"حسابداری"},
			{ (int)UnitIds.Graphics,"گرافیک"},
		};

		public static string GetUnitName(this int id)
		{
			return keyValues[id] ?? "";
		}

		public static int GetUnitId(this string name)
		{
			try
			{
				return keyValues.FirstOrDefault(x => x.Value == name).Key;
			}
			catch (Exception)
			{
				return -1;
			}
		}
	}
}
