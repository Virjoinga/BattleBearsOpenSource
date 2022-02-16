using UnityEngine;

namespace BSCore
{
	public class DataStoreFloat : BaseDataStoreService<float>
	{
		public DataStoreFloat(string key, float defaultValue)
			: base(key, defaultValue)
		{
		}

		protected override void Persist()
		{
			PlayerPrefs.SetFloat(base.Key, _value);
		}

		protected override void Fetch()
		{
			_value = PlayerPrefs.GetFloat(base.Key, base.Default);
		}
	}
}
