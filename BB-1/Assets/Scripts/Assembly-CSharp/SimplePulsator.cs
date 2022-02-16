using UnityEngine;

public class SimplePulsator : MonoBehaviour
{
	private Transform myTransform;

	private float currentScale = 1f;

	public float maxScale = 1.1f;

	private bool goingUp = true;

	public float scaleFactor = 1f;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void Update()
	{
		if (goingUp)
		{
			currentScale += scaleFactor * Time.deltaTime;
			if (currentScale > maxScale)
			{
				currentScale = maxScale;
				goingUp = false;
			}
		}
		else
		{
			currentScale -= scaleFactor * Time.deltaTime;
			if (currentScale < 1f)
			{
				currentScale = 1f;
				goingUp = true;
			}
		}
		myTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
	}
}
