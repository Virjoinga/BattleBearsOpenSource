using System;

namespace BSCore
{
	public static class DateTimeExtensions
	{
		public static DateTime Beginning
		{
			get
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			}
		}

		public static TimeSpan SinceUnix(this DateTime time)
		{
			return time - new DateTime(1970, 1, 1);
		}

		public static int ToUnixTimeStamp(this DateTime dateTime)
		{
			return (int)dateTime.Subtract(Beginning).TotalSeconds;
		}

		public static DateTime FromUnixTimeStamp(int timestamp)
		{
			return Beginning.AddSeconds(timestamp);
		}
	}
}
