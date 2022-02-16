using UnityEngine;

public class SpawnerGizmoEnd : MonoBehaviour
{
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawIcon(base.transform.position, "spawnpoint.psd");
	}
}
