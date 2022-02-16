using UnityEngine;

public class SpawnpointSpawner : MonoBehaviour
{
	public GameObject spawnObject;

	public bool isBoss;

	public bool alwaysSpawn;

	private bool hasSpawned;

	public bool onlySpawnOnce;

	public bool spawnBasedOnDifficulty;

	public GameDifficulty onlySpawnIfDifficultyAtMost = GameDifficulty.NONE;

	private void Awake()
	{
		if (alwaysSpawn)
		{
			OnSpawn();
			OnBossSpawn();
		}
	}

	public void OnSpawnOnce()
	{
		if (alwaysSpawn || isBoss || hasSpawned)
		{
			return;
		}
		if (spawnBasedOnDifficulty && GameManager.Instance.currentDifficulty > onlySpawnIfDifficultyAtMost)
		{
			hasSpawned = true;
			Object.Destroy(base.gameObject);
			return;
		}
		if (spawnObject != null)
		{
			GameObject obj = Object.Instantiate(spawnObject);
			obj.transform.parent = base.transform.parent;
			obj.transform.position = base.transform.position;
			obj.transform.rotation = base.transform.rotation;
			obj.name = spawnObject.name;
		}
		hasSpawned = true;
		Object.Destroy(base.gameObject);
	}

	public void OnSpawn()
	{
		if (isBoss || hasSpawned || onlySpawnOnce)
		{
			return;
		}
		if (spawnBasedOnDifficulty && GameManager.Instance.currentDifficulty > onlySpawnIfDifficultyAtMost)
		{
			hasSpawned = true;
			Object.Destroy(base.gameObject);
			return;
		}
		if (spawnObject != null)
		{
			GameObject gameObject = Object.Instantiate(spawnObject);
			gameObject.transform.parent = base.transform.parent;
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			gameObject.name = spawnObject.name;
			Door door = gameObject.GetComponent(typeof(Door)) as Door;
			if (door != null)
			{
				door.exitDirection = (ExitDirection)(Mathf.RoundToInt(gameObject.transform.localEulerAngles.y) / 90);
			}
		}
		hasSpawned = true;
		Object.Destroy(base.gameObject);
	}

	public void OnBossSpawn()
	{
		if (!isBoss || hasSpawned || onlySpawnOnce)
		{
			return;
		}
		if (spawnBasedOnDifficulty && GameManager.Instance.currentDifficulty > onlySpawnIfDifficultyAtMost)
		{
			hasSpawned = true;
			Object.Destroy(base.gameObject);
			return;
		}
		if (spawnObject != null)
		{
			GameObject obj = Object.Instantiate(spawnObject);
			obj.transform.parent = base.transform.parent;
			obj.transform.position = base.transform.position;
			obj.transform.rotation = base.transform.rotation;
			obj.name = spawnObject.name;
		}
		hasSpawned = true;
		Object.Destroy(base.gameObject);
	}
}
