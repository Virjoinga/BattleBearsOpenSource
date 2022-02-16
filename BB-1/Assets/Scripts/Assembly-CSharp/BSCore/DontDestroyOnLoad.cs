using UnityEngine;

namespace BSCore
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnDestroy()
		{
			Debug.Log(base.gameObject.name + " DontDestroyOnLoad was Destroy...");
		}
	}
}
