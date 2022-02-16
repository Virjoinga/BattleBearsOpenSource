using UnityEngine;

public class TubeCamera : MonoBehaviour
{
	public Camera myCam;

	private void Update()
	{
		if (Time.frameCount % 2 == 0)
		{
			myCam.enabled = false;
		}
		else
		{
			myCam.enabled = true;
		}
	}
}
