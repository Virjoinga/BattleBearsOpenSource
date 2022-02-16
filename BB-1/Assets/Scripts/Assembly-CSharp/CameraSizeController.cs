using UnityEngine;

public class CameraSizeController : MonoBehaviour
{
	private void Start()
	{
		if (GameManager.Instance.isIpad)
		{
			GetComponent<Camera>().orthographicSize = 179f;
		}
		else
		{
			GetComponent<Camera>().orthographicSize = 160f;
		}
		Object.Destroy(this);
	}
}
