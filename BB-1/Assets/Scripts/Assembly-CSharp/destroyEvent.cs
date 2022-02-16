using UnityEngine;

public class destroyEvent : MonoBehaviour
{
	public GameObject eventToDestroy;

	public void OnTriggerEnter()
	{
		Object.Destroy(eventToDestroy);
	}
}
