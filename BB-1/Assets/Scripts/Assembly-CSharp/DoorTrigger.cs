using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	private Door door;

	private void Awake()
	{
		door = base.transform.parent.parent.GetComponent(typeof(Door)) as Door;
	}

	private void OnTriggerEnter(Collider other)
	{
		door.OnTriggerEnter(other);
	}
}
