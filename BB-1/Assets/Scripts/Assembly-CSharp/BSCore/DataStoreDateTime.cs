using System;
using UnityEngine;

namespace BSCore
{
	public class DataStoreDateTime : BaseDataStoreService<DateTime>
	{
		public DataStoreDateTime(string key, DateTime defaultValue)
			: base(key, defaultValue)
		{
		}

		protected override void Persist()
		{
			PlayerPrefs.SetInt(base.Key, _value.ToUnixTimeStamp());
		}

		protected override void Fetch()
		{
			_value = DateTimeExtensions.FromUnixTimeStamp(PlayerPrefs.GetInt(base.Key, base.Default.ToUnixTimeStamp()));
		}
	}
}
