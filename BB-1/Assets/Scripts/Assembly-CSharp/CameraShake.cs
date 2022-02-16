using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private Transform myTransform;

	private Vector3 origPos;

	private float shakeTimeLeft;

	private void Awake()
	{
		myTransform = base.transform;
		origPos = myTransform.localPosition;
	}

	public void OnShake(float shakeTime)
	{
		if (shakeTimeLeft <= 0f)
		{
			shakeTimeLeft = shakeTime;
			StartCoroutine(shaker());
		}
		else
		{
			shakeTimeLeft = shakeTime;
		}
	}

	private IEnumerator shaker()
	{
		while (shakeTimeLeft > 0f)
		{
			shakeTimeLeft -= 0.05f;
			myTransform.localPosition = origPos + new Vector3(0.4f - Random.value * 0.8f, 0.4f - Random.value * 0.8f, 0.4f - Random.value * 0.8f);
			yield return new WaitForSeconds(0.05f);
		}
		myTransform.localPosition = origPos;
	}
}
