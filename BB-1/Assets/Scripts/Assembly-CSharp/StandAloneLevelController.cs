using UnityEngine;

public class StandAloneLevelController : MonoBehaviour
{
	public bool isClear;

	public RoomTile roomController;

	private Transform myTransform;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void Start()
	{
		RoomTile.currentRoomColliders.Clear();
		roomController.isClear = isClear;
		roomController.Initialize();
		if (roomController.roomMusic != "")
		{
			if (GameManager.Instance.useHighres && SoundTrackController.Instance != null)
			{
				SoundTrackController.Instance.stopMusicList();
			}
		}
		else if (SoundTrackController.Instance != null)
		{
			if (GameManager.Instance.useHighres)
			{
				SoundTrackController.Instance.startMusicList();
			}
			else
			{
				SoundTrackController.Instance.playNextMusicTrack();
			}
		}
		Object.Instantiate(GameManager.Instance.pathfinder).transform.parent = roomController.transform;
		roomController.gameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
		roomController.gameObject.BroadcastMessage("OnBossSpawn", SendMessageOptions.DontRequireReceiver);
		roomController.gameObject.BroadcastMessage("OnInitialize", SendMessageOptions.DontRequireReceiver);
	}
}
