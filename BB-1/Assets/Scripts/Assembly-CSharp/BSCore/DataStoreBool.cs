using UnityEngine;

namespace BSCore
{
	public class DataStoreBool : BaseDataStoreService<bool>
	{
		public DataStoreBool(string key, bool defaultValue)
			: base(key, defaultValue)
		{
		}

		protected override void Persist()
		{
			PlayerPrefs.SetInt(base.Key, _value ? 1 : 0);
		}

		protected override void Fetch()
		{
			_value = PlayerPrefs.GetInt(base.Key, base.Default ? 1 : 0) != 0;
		}
	}
}
