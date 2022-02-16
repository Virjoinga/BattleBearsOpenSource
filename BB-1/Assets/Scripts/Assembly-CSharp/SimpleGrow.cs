using UnityEngine;

public class SimpleGrow : MonoBehaviour
{
	public float targetScaleFactor = 5f;

	public float duration = 2f;

	private float startTime;

	private Transform myTransform;

	private Vector3 originalScale;

	private void Awake()
	{
		myTransform = base.transform;
		startTime = Time.time;
		originalScale = myTransform.localScale;
	}

	private void Update()
	{
		float num = 1f + (Time.time - startTime) / duration * (targetScaleFactor - 1f);
		if (num >= targetScaleFactor)
		{
			num = targetScaleFactor;
			Object.Destroy(this);
		}
		myTransform.localScale = originalScale * num;
	}
}
