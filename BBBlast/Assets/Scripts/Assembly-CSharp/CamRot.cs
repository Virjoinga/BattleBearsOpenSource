using UnityEngine;

public class CamRot : MonoBehaviour
{
	private bool done;

	private float myTime;

	private Transform myTransform;

	private void Start()
	{
		myTime = Time.time;
		myTransform = base.transform;
	}

	private void Update()
	{
		if (GameManager.Instance.currentGameMode != GameMode.MENU)
		{
			if (GameManager.Instance.currentLevel == Level.JUNGLE && !done)
			{
				float x = Mathf.Lerp(base.transform.localEulerAngles.x, 361f, 0.01f);
				myTransform.localEulerAngles = new Vector3(x, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
				if (Time.time > myTime + 1f)
				{
					done = true;
				}
			}
		}
		else
		{
			float x2 = Mathf.Lerp(base.transform.localEulerAngles.x, 357f, 0.01f);
			myTransform.localEulerAngles = new Vector3(x2, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
			done = false;
			myTime = Time.time;
		}
	}
}
