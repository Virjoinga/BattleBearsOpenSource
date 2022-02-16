using UnityEngine;

public class SpawnerGizmoStart : MonoBehaviour
{
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawIcon(base.transform.position, "spawnpointZ.psd");
	}
}
