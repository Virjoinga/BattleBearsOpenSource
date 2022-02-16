using UnityEngine;

public class MoreMemTest : MonoBehaviour
{
	public GameObject enemy;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.A))
		{
			plain();
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			resource();
		}
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).position.x < (float)Screen.width * 0.5f && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				plain();
			}
			if (Input.GetTouch(0).position.x > (float)Screen.width * 0.5f && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				resource();
			}
		}
	}

	private void plain()
	{
		Object.Instantiate(enemy);
	}

	private void resource()
	{
		Object.Instantiate(Resources.Load("Enemies/pinky"));
	}
}
