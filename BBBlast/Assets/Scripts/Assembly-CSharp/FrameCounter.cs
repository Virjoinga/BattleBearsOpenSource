using System;
using System.Collections;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
	private float lastTimeChecked;

	private int lastFrameCount;

	private float recalculationInterval = 1f;

	public double fps;

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
			fps = Math.Round(fps);
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(Screen.width - 30, (float)Screen.height * 0.5f, 40f, 40f), Math.Round(fps).ToString());
	}
}
