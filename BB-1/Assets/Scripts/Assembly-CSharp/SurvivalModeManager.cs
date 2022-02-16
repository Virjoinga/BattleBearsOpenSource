using System.Collections;
using UnityEngine;

public class SurvivalModeManager : ModeManager
{
	public GameObject[] survivalRoomPrefabs;

	private int[] nextRoomIndices = new int[4];

	private int lastRoomGeneratedIndex;

	private int[] randomSeeds = new int[4];

	public int increaseMinTurretsEvery = 15;

	public int increaseMaxTurretsEvery = 5;

	public int increaseWavesEvery = 10;

	public int minimumHuggablesPerWave = 5;

	public float huggableWaveIncreaseRatio = 1f;

	public GameObject[] huggables;

	private bool currentRoomCleared;

	public GameObject currentPathfinder;

	public string[] roomTextureNames;

	private Texture2D[] roomTextures;

	protected override void Awake()
	{
		base.Awake();
		currentLevel = null;
		survivalMode = true;
	}

	private void getNextRoomIndices()
	{
		for (int i = 0; i < 4; i++)
		{
			nextRoomIndices[i] = Random.Range(0, survivalRoomPrefabs.Length);
			randomSeeds[i] = Random.Range(0, 10000);
		}
	}

	public GameObject getRandomSurvivalRoom()
	{
		return survivalRoomPrefabs[Random.Range(0, survivalRoomPrefabs.Length)];
	}

	protected override void Start()
	{
		base.Start();
		roomTextures = new Texture2D[roomTextureNames.Length];
		for (int i = 0; i < roomTextureNames.Length; i++)
		{
			if (GameManager.Instance.useHighres)
			{
				roomTextures[i] = Resources.Load("Textures/High/" + roomTextureNames[i], typeof(Texture2D)) as Texture2D;
			}
			else
			{
				roomTextures[i] = Resources.Load("Textures/Low/" + roomTextureNames[i], typeof(Texture2D)) as Texture2D;
			}
		}
		handleMusic();
		currentRoomCleared = true;
		if (GameManager.Instance.isLoading)
		{
			Debug.Log("yo");
			lastRoomGeneratedIndex = PlayerPrefs.GetInt("currentRoomIndex", 0);
			string[] array = PlayerPrefs.GetString("nextRoomIndices", "").Split(' ');
			for (int j = 0; j < array.Length; j++)
			{
				if (!(array[j] == ""))
				{
					nextRoomIndices[j] = int.Parse(array[j]);
				}
			}
			string[] array2 = PlayerPrefs.GetString("randomSeeds", "").Split(' ');
			for (int k = 0; k < array2.Length; k++)
			{
				if (!(array2[k] == ""))
				{
					randomSeeds[k] = int.Parse(array2[k]);
				}
			}
			activeRoom = Object.Instantiate(survivalRoomPrefabs[lastRoomGeneratedIndex]);
			activeRoom.transform.Rotate(0f, 180f, 0f);
			activeRoom.name = "Room " + StatsManager.Instance.currentRoomNumber;
			Renderer component = activeRoom.GetComponent<Renderer>();
			if (component == null)
			{
				Transform transform = activeRoom.transform.Find("room");
				if (transform != null)
				{
					component = transform.GetComponent<Renderer>();
				}
			}
			if (component != null)
			{
				component.material.mainTexture = roomTextures[StatsManager.Instance.currentRoomNumber / 5 % roomTextures.Length];
			}
			float @float = PlayerPrefs.GetFloat("roomX");
			float float2 = PlayerPrefs.GetFloat("roomY");
			float float3 = PlayerPrefs.GetFloat("roomZ");
			activeRoom.transform.position = new Vector3(@float, float2, float3);
			roomController = activeRoom.GetComponentInChildren(typeof(RoomTile)) as RoomTile;
			roomController.isClear = true;
			currentPathfinder = Object.Instantiate(GameManager.Instance.pathfinder);
			currentPathfinder.transform.parent = roomController.transform;
			roomController.gameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			roomController.Initialize();
			roomController.gameObject.BroadcastMessage("OnCheckRoomClear", SendMessageOptions.DontRequireReceiver);
			roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			lastRoomGeneratedIndex = 1;
			activeRoom = Object.Instantiate(survivalRoomPrefabs[lastRoomGeneratedIndex]);
			activeRoom.transform.Rotate(0f, 180f, 0f);
			activeRoom.name = "Room " + StatsManager.Instance.currentRoomNumber;
			Renderer component2 = activeRoom.GetComponent<Renderer>();
			if (component2 == null)
			{
				Transform transform2 = activeRoom.transform.Find("room");
				if (transform2 != null)
				{
					component2 = transform2.GetComponent<Renderer>();
				}
			}
			if (component2 != null)
			{
				component2.material.mainTexture = roomTextures[StatsManager.Instance.currentRoomNumber / 5 % roomTextures.Length];
			}
			roomController = activeRoom.GetComponentInChildren(typeof(RoomTile)) as RoomTile;
			roomController.isClear = false;
			currentPathfinder = Object.Instantiate(GameManager.Instance.pathfinder);
			currentPathfinder.transform.parent = roomController.transform;
			populateEnemySpawner(roomController.gameObject.GetComponent(typeof(EnemySpawner)) as EnemySpawner);
			roomController.gameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			roomController.Initialize();
			roomController.gameObject.BroadcastMessage("OnCheckRoomClear", SendMessageOptions.DontRequireReceiver);
			roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
			roomController.turnOnRedLights();
			getNextRoomIndices();
		}
		HUDController.Instance.updateRoom(StatsManager.Instance.currentRoomNumber);
		currentRoomCleared = false;
	}

	private void populateEnemySpawner(EnemySpawner enemySpawner)
	{
		enemySpawner.huggables = huggables;
		enemySpawner.numGuysToSpawnPerWaveMEDIUM = new int[StatsManager.Instance.currentRoomNumber / increaseWavesEvery + 1];
		for (int i = 0; i < enemySpawner.numGuysToSpawnPerWaveMEDIUM.Length; i++)
		{
			enemySpawner.numGuysToSpawnPerWaveMEDIUM[i] = minimumHuggablesPerWave + (int)(huggableWaveIncreaseRatio * (float)StatsManager.Instance.currentRoomNumber);
		}
		enemySpawner.spawnProbabilitiesMEDIUM = new float[huggables.Length];
		float num = 0.998f;
		float num2 = 0.15f;
		float num3 = 0.55f;
		float num4 = Mathf.Min(num, num3 - (num3 - num2) * Mathf.Min(1f, (float)StatsManager.Instance.currentRoomNumber / 100f));
		if (num4 < 0f)
		{
			num4 = 0f;
		}
		num -= num4;
		float num5 = 0.1f;
		float num6 = 0.18f;
		float num7 = Mathf.Min(num, num6 - (num6 - num5) * Mathf.Min(1f, (float)StatsManager.Instance.currentRoomNumber / 100f));
		if (num7 < 0f)
		{
			num7 = 0f;
		}
		num -= num7;
		float num8 = 0.1f;
		float num9 = 0.05f;
		float num10 = Mathf.Min(num, num9 - (num9 - num8) * Mathf.Min(1f, (float)StatsManager.Instance.currentRoomNumber / 100f));
		if (num10 < 0f)
		{
			num10 = 0f;
		}
		num -= num10;
		float num11 = 0.25f;
		float num12 = 0.02f;
		float num13 = Mathf.Min(num, num12 - (num12 - num11) * Mathf.Min(1f, (float)StatsManager.Instance.currentRoomNumber / 100f));
		if (num13 < 0f)
		{
			num13 = 0f;
		}
		num -= num13;
		float num14 = 0.1f;
		float num15 = 0.05f;
		float num16 = Mathf.Min(num, num15 - (num15 - num14) * Mathf.Min(1f, (float)StatsManager.Instance.currentRoomNumber / 100f));
		if (num16 < 0f)
		{
			num16 = 0f;
		}
		num -= num16;
		float num17 = 0.1f;
		float num18 = 0.05f;
		float num19 = Mathf.Min(num, num18 - (num18 - num17) * Mathf.Min(1f, (float)StatsManager.Instance.currentRoomNumber / 100f));
		if (num19 < 0f)
		{
			num19 = 0f;
		}
		num -= num19;
		if (num < 0f)
		{
			num = 0f;
		}
		enemySpawner.spawnProbabilitiesMEDIUM[0] = num4;
		enemySpawner.spawnProbabilitiesMEDIUM[1] = num7;
		enemySpawner.spawnProbabilitiesMEDIUM[2] = num10;
		enemySpawner.spawnProbabilitiesMEDIUM[3] = num;
		enemySpawner.spawnProbabilitiesMEDIUM[4] = 0.001f;
		enemySpawner.spawnProbabilitiesMEDIUM[5] = 0f;
		enemySpawner.spawnProbabilitiesMEDIUM[6] = 1E-05f;
		enemySpawner.spawnProbabilitiesMEDIUM[7] = num13;
		enemySpawner.spawnProbabilitiesMEDIUM[8] = num16;
		enemySpawner.spawnProbabilitiesMEDIUM[9] = num19;
	}

	private void saveState()
	{
		playerController.saveState(activeRoom.transform.position, new Vector2(0f, 0f), "");
		PlayerPrefs.SetInt("currentRoomIndex", lastRoomGeneratedIndex);
		string text = "";
		for (int i = 0; i < nextRoomIndices.Length; i++)
		{
			text = text + nextRoomIndices[i] + " ";
		}
		PlayerPrefs.SetString("nextRoomIndices", text);
		string text2 = "";
		for (int j = 0; j < randomSeeds.Length; j++)
		{
			text2 = text2 + randomSeeds[j] + " ";
		}
		PlayerPrefs.SetString("randomSeeds", text2);
	}

	public override void activateNextRoom(ExitDirection dir)
	{
		theDir = dir;
		lastRoomGeneratedIndex = nextRoomIndices[(int)dir];
		nextActiveRoom = Object.Instantiate(survivalRoomPrefabs[nextRoomIndices[(int)dir]]);
		nextActiveRoom.name = "Room " + StatsManager.Instance.currentRoomNumber;
		nextActiveRoom.transform.Rotate(0f, 180f, 0f);
		Renderer component = nextActiveRoom.GetComponent<Renderer>();
		if (component == null)
		{
			Transform transform = nextActiveRoom.transform.Find("room");
			if (transform != null)
			{
				component = transform.GetComponent<Renderer>();
			}
		}
		if (component != null)
		{
			component.material.mainTexture = roomTextures[StatsManager.Instance.currentRoomNumber / 5 % roomTextures.Length];
		}
		nextRoomController = nextActiveRoom.GetComponentInChildren(typeof(RoomTile)) as RoomTile;
		nextRoomController.isClear = false;
		Random.seed = randomSeeds[(int)dir];
		RandomObjectSpawner randomObjectSpawner = nextRoomController.gameObject.GetComponentInChildren(typeof(RandomObjectSpawner)) as RandomObjectSpawner;
		if (randomObjectSpawner != null)
		{
			randomObjectSpawner.minSpawns = StatsManager.Instance.currentRoomNumber / increaseMinTurretsEvery;
			randomObjectSpawner.maxSpawns = StatsManager.Instance.currentRoomNumber / increaseMaxTurretsEvery;
			randomObjectSpawner.OnCreateSpawns();
		}
		populateEnemySpawner(nextRoomController.gameObject.GetComponent(typeof(EnemySpawner)) as EnemySpawner);
		nextRoomController.gameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		nextRoomController.Initialize();
		oppositeDir = ExitDirection.NORTH;
		switch (dir)
		{
		case ExitDirection.NORTH:
			nextActiveRoom.transform.position = activeRoom.transform.position - new Vector3(0f, 0f, 88.25f);
			oppositeDir = ExitDirection.SOUTH;
			break;
		case ExitDirection.EAST:
			nextActiveRoom.transform.position = activeRoom.transform.position + new Vector3(88.25f, 0f, 0f);
			oppositeDir = ExitDirection.WEST;
			break;
		case ExitDirection.SOUTH:
			nextActiveRoom.transform.position = activeRoom.transform.position - new Vector3(0f, 0f, -88.25f);
			oppositeDir = ExitDirection.NORTH;
			break;
		case ExitDirection.WEST:
			nextActiveRoom.transform.position = activeRoom.transform.position + new Vector3(-88.25f, 0f, 0f);
			oppositeDir = ExitDirection.EAST;
			break;
		}
		nextRoomController.disableColliders();
		otherDoor = nextRoomController.getDoor(oppositeDir);
		otherDoor.gameObject.SetActiveRecursively(false);
	}

	public override IEnumerator autoMoveToNextRoom()
	{
		if (GameManager.Instance.isHacked)
		{
			Application.OpenURL("http://www.battlebears.com/bbneg1piracy");
			yield return new WaitForSeconds(1f);
			Application.LoadLevel("MainMenu");
		}
		if (currentPathfinder != null)
		{
			Object.Destroy(currentPathfinder);
		}
		MoveToPosition mtp = PCGO.AddComponent(typeof(MoveToPosition)) as MoveToPosition;
		mtp.move(nextRoomController.GetDoorNavPoint(oppositeDir));
		while (mtp != null)
		{
			yield return new WaitForSeconds(0.01f);
		}
		StatsManager.Instance.currentRoomNumber++;
		HUDController.Instance.updateRoom(StatsManager.Instance.currentRoomNumber);
		roomController.disableColliders();
		prevActiveRoom = activeRoom;
		activeRoom = nextActiveRoom;
		roomController = nextRoomController;
		ExitDirection exitDirection = theDir;
		theDir = oppositeDir;
		oppositeDir = exitDirection;
		despawnCurrentRoom();
		getNextRoomIndices();
		roomController.enableColliders();
		otherDoor.gameObject.SetActiveRecursively(true);
		roomController.setDoorWeCameFrom(theDir);
		Transform doorNavTransform = roomController.GetDoorNavTransform(theDir);
		playerController.OnSetSpawn(doorNavTransform);
		handleMusic();
		currentPathfinder = Object.Instantiate(GameManager.Instance.pathfinder);
		currentPathfinder.transform.parent = roomController.transform;
		roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
		roomController.gameObject.BroadcastMessage("OnCheckRoomClear", SendMessageOptions.DontRequireReceiver);
		if (!roomController.isClear)
		{
			roomController.turnOnRedLights();
		}
		else
		{
			roomController.turnOnGreenLights();
		}
		playCloseDoorSound();
		currentRoomCleared = false;
	}

	public override void setRoomCleared()
	{
		if (!currentRoomCleared)
		{
			currentRoomCleared = true;
			if (StatsManager.Instance.currentRoomNumber != 5 && StatsManager.Instance.currentRoomNumber != 15 && StatsManager.Instance.currentRoomNumber != 30 && StatsManager.Instance.currentRoomNumber != 50 && StatsManager.Instance.currentRoomNumber != 75 && StatsManager.Instance.currentRoomNumber != 100 && StatsManager.Instance.currentRoomNumber != 120 && StatsManager.Instance.currentRoomNumber != 175 && StatsManager.Instance.currentRoomNumber != 200)
			{
				int currentRoomNumber = StatsManager.Instance.currentRoomNumber;
				int num = 255;
			}
			HUDController.Instance.OnRoomClearFirstTime();
			if (playerController.currentSatelliteController != null)
			{
				StatsManager.Instance.numberOfRoomsWithSatellite++;
				int numberOfRoomsWithSatellite = StatsManager.Instance.numberOfRoomsWithSatellite;
				int num2 = 4;
			}
			saveState();
			roomController.roomClear();
		}
	}
}
