using UnityEngine;

public class TutorialRoomSpawner : MonoBehaviour
{
	private Door mainDoor;

	public GameObject huggable;

	private bool doneSpawning;

	private void Start()
	{
		mainDoor = base.transform.root.GetComponentInChildren(typeof(Door)) as Door;
		if (GameManager.Instance.inTutorialRoom)
		{
			mainDoor.disableDoor();
		}
	}

	private void OnTriggerEnter()
	{
		if (doneSpawning)
		{
			return;
		}
		if (GameManager.Instance.inTutorialRoom)
		{
			EnemySpawner enemySpawner = base.transform.root.gameObject.AddComponent(typeof(EnemySpawner)) as EnemySpawner;
			enemySpawner.huggables = new GameObject[1];
			enemySpawner.huggables[0] = huggable;
			enemySpawner.numGuysToSpawnPerWaveEASY = new int[1];
			enemySpawner.numGuysToSpawnPerWaveMEDIUM = new int[1];
			enemySpawner.numGuysToSpawnPerWaveHARD = new int[1];
			for (int i = 0; i < enemySpawner.numGuysToSpawnPerWaveEASY.Length; i++)
			{
				enemySpawner.numGuysToSpawnPerWaveEASY[i] = 3;
			}
			for (int j = 0; j < enemySpawner.numGuysToSpawnPerWaveMEDIUM.Length; j++)
			{
				enemySpawner.numGuysToSpawnPerWaveMEDIUM[j] = 3;
			}
			for (int k = 0; k < enemySpawner.numGuysToSpawnPerWaveHARD.Length; k++)
			{
				enemySpawner.numGuysToSpawnPerWaveHARD[k] = 3;
			}
			enemySpawner.spawnProbabilitiesEASY = new float[1];
			enemySpawner.spawnProbabilitiesMEDIUM = new float[1];
			enemySpawner.spawnProbabilitiesHARD = new float[1];
			enemySpawner.spawnProbabilitiesEASY[0] = 1f;
			enemySpawner.spawnProbabilitiesMEDIUM[0] = 1f;
			enemySpawner.spawnProbabilitiesHARD[0] = 1f;
			enemySpawner.initialSpawnDelay = 0.2f;
			enemySpawner.spawnDelay = 2.5f;
			enemySpawner.SendMessage("OnCheckRoomClear");
			doneSpawning = true;
		}
		Object.Destroy(base.gameObject);
	}
}
