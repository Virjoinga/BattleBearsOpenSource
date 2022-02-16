using UnityEngine;

public class TimeyWimey : MonoBehaviour
{
	private static TimeyWimey instance;

	private bool lerpIt;

	private float timeRate;

	private float lerpToTime;

	public static TimeyWimey Instance
	{
		get
		{
			return instance;
		}
	}

	private void Start()
	{
		instance = this;
		lerpIt = false;
		timeRate = 0f;
		lerpToTime = 0f;
	}

	public void SetTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
	}

	public void SetTimeScaleOverTime(float timeScale, float toTime)
	{
		lerpToTime = timeScale;
		timeRate = toTime;
		lerpIt = true;
	}

	private void Update()
	{
		if (lerpIt)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, lerpToTime, timeRate);
			if (Time.timeScale == lerpToTime)
			{
				lerpIt = false;
			}
		}
	}
}
