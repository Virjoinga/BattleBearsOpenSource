using System.Collections;
using UnityEngine;

public class TimedObjectDestroy : MonoBehaviour
{
	public float timeUntilDestroy = 0.2f;

	private void Start()
	{
		StartCoroutine(delayedDestroy(timeUntilDestroy));
	}

	private IEnumerator delayedDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Object.Destroy(base.gameObject);
	}
}
