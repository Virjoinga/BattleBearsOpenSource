using UnityEngine;

public class TempLevelActivator : MonoBehaviour
{
	private void Awake()
	{
		if (GameManager.Instance.currentLevel.ToString() == base.gameObject.name)
		{
			//base.gameObject.active = true;
			base.gameObject.SetActive(true);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
