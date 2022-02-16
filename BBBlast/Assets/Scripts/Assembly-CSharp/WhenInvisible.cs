using UnityEngine;

public class WhenInvisible : MonoBehaviour
{
	private bool isSeen;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnBecameVisible()
	{
		isSeen = true;
	}

	private void OnBecameInvisible()
	{
		if (isSeen)
		{
			SendMessageUpwards("notSeen", SendMessageOptions.DontRequireReceiver);
			isSeen = false;
		}
	}
}
