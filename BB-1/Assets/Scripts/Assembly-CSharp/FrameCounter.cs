using System.Collections;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
	private float lastTimeChecked;

	private int lastFrameCount;

	private float recalculationInterval = 1f;

	public float fps;

	private void Awake()
	{
		StartCoroutine(calculateFrames());
	}

	private IEnumerator calculateFrames()
	{
		while (true)
		{
			lastFrameCount = Time.frameCount;
			lastTimeChecked = Time.realtimeSinceStartup;
			yield return new WaitForSeconds(recalculationInterval);
			fps = (float)(Time.frameCount - lastFrameCount) / (Time.realtimeSinceStartup - lastTimeChecked);
		}
	}
}
