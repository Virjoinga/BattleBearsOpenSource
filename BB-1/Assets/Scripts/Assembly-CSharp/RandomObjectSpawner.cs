using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
	public int minSpawns;

	public int maxSpawns = 1;

	public GameObject[] objectsToSpawn;

	private Transform myTransform;

	private void Awake()
	{
		myTransform = base.transform;
	}

	public void OnCreateSpawns()
	{
		int num = Random.Range(minSpawns, maxSpawns + 1);
		if (num > 0)
		{
			if (num >= myTransform.childCount)
			{
				foreach (Transform item in myTransform)
				{
					(item.gameObject.AddComponent(typeof(SpawnpointSpawner)) as SpawnpointSpawner).spawnObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
					item.parent = myTransform.parent;
				}
			}
			else
			{
				while (num > 0)
				{
					int num2 = Random.Range(0, myTransform.childCount);
					int num3 = 0;
					foreach (Transform item2 in myTransform)
					{
						if (num3 == num2)
						{
							(item2.gameObject.AddComponent(typeof(SpawnpointSpawner)) as SpawnpointSpawner).spawnObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
							item2.parent = myTransform.parent;
							break;
						}
						num3++;
					}
					num--;
				}
			}
		}
		Object.Destroy(base.gameObject);
	}
}
