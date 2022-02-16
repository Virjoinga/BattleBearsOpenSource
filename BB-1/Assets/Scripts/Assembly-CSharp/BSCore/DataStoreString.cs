using UnityEngine;

namespace BSCore
{
	public class DataStoreString : BaseDataStoreService<string>
	{
		public DataStoreString(string key, string defaultValue)
			: base(key, defaultValue)
		{
		}

		protected override void Persist()
		{
			PlayerPrefs.SetString(base.Key, _value);
		}

		protected override void Fetch()
		{
			_value = PlayerPrefs.GetString(base.Key, base.Default);
		}
	}
}
