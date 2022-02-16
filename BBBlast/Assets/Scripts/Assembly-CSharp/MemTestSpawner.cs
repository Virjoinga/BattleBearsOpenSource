using UnityEngine;

public class MemTestSpawner : MonoBehaviour
{
	private float myTime;

	public GameObject arrayTest;

	private void Start()
	{
		myTime = Time.time;
	}

	private void Update()
	{
		if (Time.time > myTime + 2.5f)
		{
			Object.Instantiate(arrayTest);
			myTime = Time.time;
		}
	}
}
