using UnityEngine;

namespace BSCore
{
	public class DataStoreInt : BaseDataStoreService<int>
	{
		public DataStoreInt(string key, int defaultValue)
			: base(key, defaultValue)
		{
		}

		protected override void Persist()
		{
			PlayerPrefs.SetInt(base.Key, _value);
		}

		protected override void Fetch()
		{
			_value = PlayerPrefs.GetInt(base.Key, base.Default);
		}
	}
}
