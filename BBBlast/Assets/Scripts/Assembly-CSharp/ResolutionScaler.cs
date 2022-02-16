using UnityEngine;

public class ResolutionScaler : MonoBehaviour
{
	public float normal;

	public float extra;

	private Transform myTransform;

	private void Awake()
	{
		myTransform = base.transform;
		if (Screen.height == 800)
		{
			myTransform.localScale = new Vector3(normal, myTransform.localScale.y, myTransform.localScale.z);
		}
		if (Screen.height == 854)
		{
			myTransform.localScale = new Vector3(extra, myTransform.localScale.y, myTransform.localScale.z);
		}
		Object.Destroy(this);
	}
}
