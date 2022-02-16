using System.Collections;
using UnityEngine;

public class LevelManager : ModeManager
{
	private LevelCell currentLocation;

	private LevelCell nextCell;

	private Hashtable isClear;

	private bool duringLoad;

	public GameObject currentPathfinder;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		if (GameManager.Instance.isLoading)
		{
			duringLoad = true;
			StartCoroutine(endLoad());
			float @float = PlayerPrefs.GetFloat("cellX");
			float float2 = PlayerPrefs.GetFloat("cellY");
			currentLocation = currentLevel.GetCell(new Vector2(@float, float2));
			activeRoom = Object.Instantiate(currentLocation.roomTile.gameObject);
			activeRoom.transform.Rotate(0f, 180f, 0f);
			float float3 = PlayerPrefs.GetFloat("roomX");
			float float4 = PlayerPrefs.GetFloat("roomY");
			float float5 = PlayerPrefs.GetFloat("roomZ");
			activeRoom.transform.position = new Vector3(float3, float4, float5);
			activeRoom.name = currentLocation.TilePosition.ToString();
			roomController = activeRoom.GetComponentInChildren(typeof(RoomTile)) as RoomTile;
			roomController.owningCell = currentLocation;
			isClear = new Hashtable();
			string[] array = PlayerPrefs.GetString("clearedRooms", "").Split('#');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != "")
				{
					Debug.Log(array[i]);
					isClear.Add(array[i], true);
				}
			}
			currentPathfinder = Object.Instantiate(GameManager.Instance.pathfinder);
			currentPathfinder.transform.parent = roomController.transform;
			roomController.isClear = true;
			roomController.gameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			roomController.Initialize();
			roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			currentLocation = currentLevel.GetStartingRoom();
			activeRoom = Object.Instantiate(currentLocation.roomTile.gameObject);
			activeRoom.transform.Rotate(0f, 180f, 0f);
			activeRoom.name = currentLocation.TilePosition.ToString();
			roomController = activeRoom.GetComponentInChildren(typeof(RoomTile)) as RoomTile;
			roomController.owningCell = currentLocation;
			isClear = new Hashtable();
			if (GameManager.Instance.startingStage > 1)
			{
				roomController.isClear = true;
				StatsManager.Instance.currentRoomsVisited++;
			}
			else if (GameManager.Instance.inTutorialRoom)
			{
				roomController.isClear = false;
			}
			else
			{
				roomController.isClear = true;
				StatsManager.Instance.currentRoomsVisited++;
			}
			currentPathfinder = Object.Instantiate(GameManager.Instance.pathfinder);
			currentPathfinder.transform.parent = roomController.transform;
			if (!roomController.isClear)
			{
				roomController.gameObject.BroadcastMessage("OnSpawnOnce", SendMessageOptions.DontRequireReceiver);
			}
			roomController.gameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			roomController.Initialize();
			roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
			StartCoroutine(delayedStartSave());
		}
		handleMusic();
	}

	private IEnumerator delayedStartSave()
	{
		yield return new WaitForSeconds(0.1f);
		save();
	}

	private IEnumerator endLoad()
	{
		yield return new WaitForSeconds(0.2f);
		duringLoad = false;
		GameManager.Instance.isLoading = false;
	}

	public void save()
	{
		if (GameManager.Instance.inTutorialRoom || duringLoad || GameManager.Instance.inOliverBossRoom || GameManager.Instance.inRiggsBossRoom)
		{
			return;
		}
		string text = "";
		foreach (string key in isClear.Keys)
		{
			text = text + key + "#";
		}
		playerController.saveState(activeRoom.transform.position, currentLocation.TilePosition, text);
	}

	public override void activateNextRoom(ExitDirection dir)
	{
		save();
		theDir = dir;
		nextCell = currentLevel.GetNeighbourCell(currentLocation, dir);
		if (nextCell == null)
		{
			Debug.Log("trying to walk through a door to nowhere, this should not happen!");
		}
		else if (!(nextActiveRoom != null) || !(nextActiveRoom.name == nextCell.TilePosition.ToString()))
		{
			nextActiveRoom = Object.Instantiate(nextCell.roomTile.gameObject);
			nextActiveRoom.name = nextCell.TilePosition.ToString();
			nextActiveRoom.transform.Rotate(0f, 180f, 0f);
			nextRoomController = nextActiveRoom.GetComponentInChildren(typeof(RoomTile)) as RoomTile;
			nextRoomController.owningCell = nextCell;
			if (!isClear.ContainsKey(nextActiveRoom.name))
			{
				isClear.Add(nextActiveRoom.name, false);
			}
			nextRoomController.isClear = (bool)isClear[nextActiveRoom.name];
			if (!nextRoomController.isClear)
			{
				nextRoomController.gameObject.BroadcastMessage("OnSpawnOnce", SendMessageOptions.DontRequireReceiver);
			}
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
			otherDoor.gameObject.SetActive(false);
		}
	}

	public override IEnumerator autoMoveToNextRoom()
	{
		if (GameManager.Instance.isHacked)
		{
			Application.OpenURL("http://www.battlebears.com/bbneg1piracy");
			yield return new WaitForSeconds(1f);
			Application.LoadLevel("MainMenu");
		}
		if (nextRoomController == null)
		{
			yield break;
		}
		GameManager.Instance.inTutorialRoom = false;
		if (currentPathfinder != null)
		{
			Object.Destroy(currentPathfinder);
		}
		MoveToPosition mtp = PCGO.AddComponent(typeof(MoveToPosition)) as MoveToPosition;
		mtp.move(nextRoomController.GetDoorNavPoint(oppositeDir));
		inMoveOutDoor = true;
		while (mtp != null)
		{
			yield return new WaitForSeconds(0.01f);
		}
		inMoveOutDoor = false;
		roomController.disableColliders();
		if (!(nextRoomController == null))
		{
			prevActiveRoom = activeRoom;
			activeRoom = nextActiveRoom;
			roomController = nextRoomController;
			currentLocation = nextCell;
			ExitDirection exitDirection = theDir;
			theDir = oppositeDir;
			oppositeDir = exitDirection;
			roomController.enableColliders();
			otherDoor.gameObject.SetActiveRecursively(true);
			Transform doorNavTransform = roomController.GetDoorNavTransform(theDir);
			if (doorNavTransform == null)
			{
				Debug.Log(string.Concat("autoMoveToNextRoom GetDoorNavTransform theDir=", theDir, " = null"));
			}
			HUDController.Instance.OnMoveRooms(theDir);
			playerController.OnSetSpawn(doorNavTransform);
			handleMusic();
			despawnCurrentRoom();
			roomController.getDoor(theDir).setPlayOpenSound(false);
			currentPathfinder = Object.Instantiate(GameManager.Instance.pathfinder);
			currentPathfinder.transform.parent = roomController.transform;
			roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
			roomController.gameObject.BroadcastMessage("OnCheckRoomClear", SendMessageOptions.DontRequireReceiver);
			if (!roomController.isClear)
			{
				HUDController.Instance.OnHideMinimap();
				roomController.getDoor(theDir).setPlayOpenSound(true);
				roomController.turnOnRedLights();
				playCloseDoorSound();
			}
			else
			{
				roomController.turnOnGreenLights();
			}
		}
	}

	public override void setRoomCleared()
	{
		HUDController.Instance.OnRoomCleared();
		if (!isClear.ContainsKey(roomController.name) || !(bool)isClear[roomController.name])
		{
			HUDController.Instance.OnRoomClearFirstTime();
			if (playerController.currentSatelliteController != null)
			{
				StatsManager.Instance.numberOfRoomsWithSatellite++;
				int numberOfRoomsWithSatellite = StatsManager.Instance.numberOfRoomsWithSatellite;
				int num = 4;
			}
			StatsManager.Instance.currentRoomsVisited++;
			int currentRoomsVisited = StatsManager.Instance.currentRoomsVisited;
			int num2 = currentLevel.cells.Length - 1;
		}
		isClear[roomController.name] = true;
		save();
		roomController.roomClear();
	}
}
