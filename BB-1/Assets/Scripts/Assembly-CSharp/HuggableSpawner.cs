using UnityEngine;

public class HuggableSpawner : MonoBehaviour
{
	public GameObject[] huggables;

	private bool hasSpawned;

	private void OnTriggerEnter(Collider c)
	{
		if (!hasSpawned)
		{
			hasSpawned = true;
			int num = Random.Range(0, huggables.Length);
			GameObject obj = Object.Instantiate(huggables[num]);
			Vector3 position = base.transform.position;
			obj.transform.parent = (Object.FindObjectOfType(typeof(RoomTile)) as RoomTile).transform;
			position.y = 0f;
			obj.transform.position = position;
			obj.name = huggables[num].name;
			obj.SendMessage("OnActivate");
		}
	}
}
