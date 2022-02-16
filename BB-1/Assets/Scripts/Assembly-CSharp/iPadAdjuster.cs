using UnityEngine;

public class iPadAdjuster : MonoBehaviour
{
	public Vector3 pos;

	public bool destroyIfNotIpad;

	public bool adjustPosition = true;

	private void Awake()
	{
		if (GameManager.Instance.isIpad && adjustPosition)
		{
			base.transform.localPosition += pos;
		}
		if (destroyIfNotIpad && !GameManager.Instance.isIpad)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			Object.Destroy(this);
		}
	}
}
