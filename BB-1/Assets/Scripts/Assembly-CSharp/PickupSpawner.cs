using System.Collections;
using AstarClasses;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
	private AstarPath pathfinder;

	public float minSpawnTime = 3f;

	public float maxSpawnTime = 10f;

	public float[] spawnProbabilities;

	public GameObject[] spawnObjects;

	private Transform myTransform;

	private void Start()
	{
		myTransform = base.transform;
	}

	private void OnInitialize()
	{
		pathfinder = GetComponentInChildren(typeof(AstarPath)) as AstarPath;
		StartCoroutine(pickupSpawner());
	}

	private IEnumerator pickupSpawner()
	{
		while (true)
		{
			yield return new WaitForSeconds(minSpawnTime + Random.value * (maxSpawnTime - minSpawnTime));
			spawnPickup();
		}
	}

	private void spawnPickup()
	{
		float num = 0f;
		float value = Random.value;
		for (int i = 0; i < spawnProbabilities.Length; i++)
		{
			num += spawnProbabilities[i];
			if (value <= num)
			{
				if (GameManager.Instance.hasAcquiredSpecial || !(spawnObjects[i].name == "specialPickup"))
				{
					GameObject obj = Object.Instantiate(spawnObjects[i]);
					obj.transform.parent = myTransform;
					obj.transform.position = getRandomSpawnpoint();
					obj.transform.rotation = Quaternion.identity;
				}
				return;
			}
		}
		Debug.Log("shouldn't ever get here, if we have, your probabilities likely do not add up to 1!");
	}

	public Vector3 getRandomSpawnpoint()
	{
		for (int i = 0; i < 10; i++)
		{
			int num = Random.Range(0, pathfinder.grids[0].width);
			int num2 = Random.Range(0, pathfinder.grids[0].depth);
			Node node = pathfinder.staticNodes[0][num, num2];
			if (node.walkable)
			{
				return new Vector3(node.vectorPos.x, node.vectorPos.y + 2f, node.vectorPos.z);
			}
		}
		Debug.Log("ERROR: no spawn point for pickup?");
		return new Vector3(0f, -10000f, 0f);
	}
}
