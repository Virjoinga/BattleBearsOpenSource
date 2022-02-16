using UnityEngine;

public class DoorHitZone : MonoBehaviour
{
	private Door door;

	private void Awake()
	{
		door = base.transform.parent.GetComponent(typeof(Door)) as Door;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!(other.tag != "Player") && (!(ModeManager.Instance != null) || ModeManager.Instance.doorsAreUnlocked()))
		{
			door.openDoor(other.transform);
		}
	}
}
