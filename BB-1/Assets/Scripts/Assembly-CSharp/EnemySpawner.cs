using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private Component[] spawnDoors;

	private Transform[] spawnPoints;

	public ArrayList livingBadguys = new ArrayList();

	private static EnemySpawner instance;

	public int[] numGuysToSpawnPerWaveEASY;

	public int[] numGuysToSpawnPerWaveMEDIUM;

	public int[] numGuysToSpawnPerWaveHARD;

	private int currentWave;

	private int nextWaveKillsNeeded;

	private int currentWaveKills;

	public float nextWaveKillPercentage = 0.75f;

	private bool doneSpawning;

	public float initialSpawnDelay = 2f;

	public float spawnDelay = 0.2f;

	public GameObject[] huggables;

	public float[] spawnProbabilitiesEASY;

	public float[] spawnProbabilitiesMEDIUM;

	public float[] spawnProbabilitiesHARD;

	private int numGuysSpawned;

	private Transform myTransform;

	public static EnemySpawner Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		myTransform = base.transform;
		livingBadguys.Clear();
	}

	private bool OnCheckRoomClear()
	{
		if (livingBadguys.Count == 0)
		{
			switch (GameManager.Instance.currentDifficulty)
			{
			case GameDifficulty.EASY:
				if (numGuysToSpawnPerWaveEASY.Length == 0)
				{
					if (ModeManager.Instance != null)
					{
						ModeManager.Instance.setRoomCleared();
					}
					return true;
				}
				break;
			case GameDifficulty.MEDIUM:
				if (numGuysToSpawnPerWaveMEDIUM.Length == 0)
				{
					if (ModeManager.Instance != null)
					{
						ModeManager.Instance.setRoomCleared();
					}
					return true;
				}
				break;
			case GameDifficulty.HARD:
				if (numGuysToSpawnPerWaveHARD.Length == 0)
				{
					if (ModeManager.Instance != null)
					{
						ModeManager.Instance.setRoomCleared();
					}
					return true;
				}
				break;
			}
		}
		StartCoroutine(initialDelay());
		return false;
	}

	private IEnumerator initialDelay()
	{
		spawnDoors = GetComponentsInChildren(typeof(Door));
		spawnPoints = new Transform[spawnDoors.Length];
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			spawnPoints[i] = (spawnDoors[i] as Door).getSpawnPoint();
		}
		if (ModeManager.Instance != null)
		{
			if (!ModeManager.Instance.doorsAreUnlocked() && !GameManager.Instance.noBadguys)
			{
				yield return new WaitForSeconds(initialSpawnDelay);
				StartCoroutine(spawnNextWave(currentWave));
			}
			else
			{
				doneSpawning = true;
				checkRoomClear();
			}
		}
	}

	private IEnumerator spawnNextWave(int waveIndex)
	{
		int numWaves = 0;
		int[] numGuysToSpawnPerWave = new int[1];
		switch (GameManager.Instance.currentDifficulty)
		{
		case GameDifficulty.EASY:
		{
			numWaves = numGuysToSpawnPerWaveEASY.Length;
			int[] array = new int[numWaves];
			numGuysToSpawnPerWave = numGuysToSpawnPerWaveEASY;
			break;
		}
		case GameDifficulty.MEDIUM:
		{
			numWaves = numGuysToSpawnPerWaveMEDIUM.Length;
			int[] array2 = new int[numWaves];
			numGuysToSpawnPerWave = numGuysToSpawnPerWaveMEDIUM;
			break;
		}
		case GameDifficulty.HARD:
		{
			numWaves = numGuysToSpawnPerWaveHARD.Length;
			int[] array3 = new int[numWaves];
			numGuysToSpawnPerWave = numGuysToSpawnPerWaveHARD;
			break;
		}
		}
		if (numWaves == 0)
		{
			doneSpawning = true;
		}
		else
		{
			if (waveIndex > numWaves - 1)
			{
				yield break;
			}
			nextWaveKillsNeeded = (int)((float)numGuysToSpawnPerWave[waveIndex] * nextWaveKillPercentage);
			currentWaveKills = 0;
			int i = 0;
			while (i < numGuysToSpawnPerWave[waveIndex])
			{
				yield return new WaitForSeconds(spawnDelay);
				while (livingBadguys.Count >= GameManager.Instance.maxGuysAlive - 1)
				{
					yield return new WaitForSeconds(0.1f);
				}
				spawnGuy();
				int num = i + 1;
				i = num;
			}
			if (waveIndex == numWaves - 1)
			{
				doneSpawning = true;
				yield return new WaitForSeconds(1f);
			}
		}
	}

	private void spawnGuy()
	{
		int num = Random.Range(0, spawnDoors.Length);
		Door door = spawnDoors[num] as Door;
		StartCoroutine(door.spawningOpen());
		float num2 = 0f;
		float value = Random.value;
		float[] array = new float[1];
		switch (GameManager.Instance.currentDifficulty)
		{
		case GameDifficulty.EASY:
			array = new float[spawnProbabilitiesEASY.Length];
			array = spawnProbabilitiesEASY;
			break;
		case GameDifficulty.MEDIUM:
			array = new float[spawnProbabilitiesMEDIUM.Length];
			array = spawnProbabilitiesMEDIUM;
			break;
		case GameDifficulty.HARD:
			array = new float[spawnProbabilitiesHARD.Length];
			array = spawnProbabilitiesHARD;
			break;
		}
		for (int i = 0; i < array.Length; i++)
		{
			num2 += array[i];
			if (value <= num2)
			{
				GameObject gameObject = Object.Instantiate(huggables[i]);
				gameObject.transform.parent = myTransform;
				gameObject.transform.position = spawnPoints[num].position + spawnPoints[num].forward * 10f;
				gameObject.transform.rotation = Quaternion.identity;
				gameObject.name = huggables[i].name;
				gameObject.SendMessage("OnSetStartPoint", spawnPoints[num].position);
				numGuysSpawned++;
				livingBadguys.Add(gameObject);
				break;
			}
		}
	}

	public void removeEnemy(GameObject obj)
	{
		livingBadguys.Remove(obj);
		currentWaveKills++;
		if (currentWaveKills >= nextWaveKillsNeeded && !doneSpawning)
		{
			currentWave++;
			StartCoroutine(spawnNextWave(currentWave));
		}
		checkRoomClear();
	}

	private void checkRoomClear()
	{
		if (livingBadguys.Count <= 0 && doneSpawning && ModeManager.Instance != null)
		{
			Component[] array = spawnDoors;
			foreach (Component obj in array)
			{
				(obj as Door).StopAllCoroutines();
				(obj as Door).OnClose();
			}
			SendMessage("OnRoomCleared", SendMessageOptions.DontRequireReceiver);
			ModeManager.Instance.setRoomCleared();
		}
	}

	public void addTurret(GameObject turret)
	{
		livingBadguys.Add(turret);
	}

	public void removeTurret(GameObject turret)
	{
		livingBadguys.Remove(turret);
		checkRoomClear();
	}
}
