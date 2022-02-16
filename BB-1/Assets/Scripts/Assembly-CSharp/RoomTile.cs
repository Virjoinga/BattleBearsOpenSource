using System.Collections;
using UnityEngine;

public class RoomTile : MonoBehaviour
{
	public Transform turretsGroup;

	public Transform wallsGroup;

	private ArrayList valuableDoors;

	public int roomID;

	public string roomTitle;

	public Door[] doors = new Door[0];

	public bool isClear;

	public LevelCell owningCell;

	private Transform myTransform;

	public static ArrayList currentRoomColliders = new ArrayList();

	public string roomMusic = "";

	private Door doorWeCameFrom;

	public void setDoorWeCameFrom(ExitDirection d)
	{
		doorWeCameFrom = getDoor(d);
		doorWeCameFrom.disableDoor();
	}

	public void Awake()
	{
		myTransform = base.transform;
	}

	public void Initialize()
	{
		Component[] componentsInChildren = GetComponentsInChildren(typeof(Collider), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			currentRoomColliders.Add(componentsInChildren[i] as Collider);
		}
		if (!wallsGroup)
		{
			wallsGroup = myTransform.Find("WALLS");
		}
		if (doors.Length < 1)
		{
			Component[] componentsInChildren2 = GetComponentsInChildren(typeof(Door), true);
			valuableDoors = new ArrayList();
			Component[] array = componentsInChildren2;
			for (int j = 0; j < array.Length; j++)
			{
				Door door = (Door)array[j];
				if (ModeManager.Instance != null)
				{
					if (ModeManager.Instance.currentLevel != null)
					{
						if (ModeManager.Instance.currentLevel.GetNeighbourCell(owningCell, door.exitDirection) != null)
						{
							valuableDoors.Add(door);
						}
						else
						{
							door.disableDoor();
						}
					}
					else if (door != doorWeCameFrom)
					{
						valuableDoors.Add(door);
					}
					else
					{
						door.disableDoor();
					}
				}
				else
				{
					door.disableDoor();
				}
			}
			if (valuableDoors.Count > 0)
			{
				doors = new Door[valuableDoors.Count];
				for (int k = 0; k < doors.Length; k++)
				{
					doors[k] = valuableDoors[k] as Door;
					if (doors[k] != doorWeCameFrom)
					{
						doors[k].enableDoor();
						if (isClear)
						{
							doors[k].turnOnGreenLight();
						}
					}
				}
			}
		}
		if (isClear)
		{
			Component[] componentsInChildren3 = base.gameObject.GetComponentsInChildren(typeof(TurretController), true);
			if (componentsInChildren3 != null)
			{
				Component[] array = componentsInChildren3;
				for (int j = 0; j < array.Length; j++)
				{
					TurretController obj = (TurretController)array[j];
					obj.createDestroyedTurret();
					Object.Destroy(obj.gameObject);
				}
			}
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("DestroyOnLoad");
		for (int j = 0; j < array2.Length; j++)
		{
			Object.Destroy(array2[j]);
		}
	}

	public Door getDoor(ExitDirection dir)
	{
		Door[] array = doors;
		foreach (Door door in array)
		{
			if (door.exitDirection == dir)
			{
				return door;
			}
		}
		return null;
	}

	public Vector3 GetDoorNavPoint(ExitDirection dir)
	{
		Door[] array = doors;
		foreach (Door door in array)
		{
			if (door.exitDirection == dir)
			{
				return door.getTransitionPoint().position;
			}
		}
		return new Vector3(-10000f, -10000f, -10000f);
	}

	public Transform GetDoorNavTransform(ExitDirection dir)
	{
		Door[] array = doors;
		foreach (Door door in array)
		{
			if (door.exitDirection == dir)
			{
				return door.getTransitionPoint();
			}
		}
		Debug.Log(" GetDoorNavTransform null ");
		return null;
	}

	public void disableColliders()
	{
		wallsGroup.gameObject.SetActiveRecursively(false);
	}

	public void enableColliders()
	{
		if (wallsGroup == null)
		{
			wallsGroup = base.transform.Find("WALLS");
		}
		wallsGroup.gameObject.SetActiveRecursively(true);
	}

	private void OnDisable()
	{
		Component[] componentsInChildren = GetComponentsInChildren(typeof(Collider), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			currentRoomColliders.Remove(componentsInChildren[i] as Collider);
		}
	}

	public void disableDoorColliders()
	{
		Door[] array = doors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].disableDoor();
		}
	}

	public void enableDoorColliders()
	{
		Door[] array = doors;
		foreach (Door door in array)
		{
			if (door != doorWeCameFrom)
			{
				door.enableDoor();
			}
		}
	}

	public void turnOnGreenLights()
	{
		Door[] array = doors;
		foreach (Door door in array)
		{
			if (door != doorWeCameFrom)
			{
				door.turnOnGreenLight();
			}
		}
	}

	public void turnOnRedLights()
	{
		Door[] array = doors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].turnOnRedLight();
		}
	}

	public void roomClear()
	{
		isClear = true;
		enableDoorColliders();
		turnOnGreenLights();
	}
}
