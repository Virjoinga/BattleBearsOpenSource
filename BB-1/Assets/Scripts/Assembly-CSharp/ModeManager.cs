using System.Collections;
using UnityEngine;

public abstract class ModeManager : MonoBehaviour
{
	public bool survivalMode;

	public Level currentLevel;

	[HideInInspector]
	public bool inMoveOutDoor;

	protected GameObject activeRoom;

	protected RoomTile roomController;

	protected PlayerController playerController;

	protected GameObject PCGO;

	protected DoorHitZone doorHitZone;

	protected float moveSpeed = 10f;

	protected GameObject nextActiveRoom;

	protected RoomTile nextRoomController;

	protected GameObject prevActiveRoom;

	protected ExitDirection theDir;

	protected ExitDirection oppositeDir;

	protected Door currDoor;

	protected Door otherDoor;

	public AudioClip doorOpens;

	public AudioClip doorClose;

	private AudioClip[] music;

	private static ModeManager instance;

	public static ModeManager Instance
	{
		get
		{
			return instance;
		}
	}

	protected virtual void Awake()
	{
		instance = this;
		music = GameManager.Instance.musicTracks;
	}

	public GameObject getCurrentRoom()
	{
		return roomController.gameObject;
	}

	protected virtual void Start()
	{
		playerController = Object.FindObjectOfType(typeof(PlayerController)) as PlayerController;
		PCGO = playerController.gameObject;
	}

	protected void handleMusic()
	{
		StartCoroutine(delayedMusicStart());
	}

	private IEnumerator delayedMusicStart()
	{
		yield return new WaitForSeconds(0.2f);
		if (GameManager.Instance.useHighres && SoundTrackController.Instance != null)
		{
			SoundTrackController.Instance.stopMusicList();
		}
		if (SoundManager.Instance.musicAudio.isPlaying)
		{
			yield break;
		}
		if ((!GameManager.Instance.inTutorialRoom && GameManager.Instance.currentCharacter == Character.OLIVER) || GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			switch (GameManager.Instance.currentStage)
			{
			case 1:
				SoundManager.Instance.playMusic(music[0], true);
				break;
			case 2:
				SoundManager.Instance.playMusic(music[1], true);
				break;
			case 3:
				SoundManager.Instance.playMusic(music[2], true);
				break;
			}
		}
		else if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			switch (GameManager.Instance.currentStage)
			{
			case 1:
				SoundManager.Instance.playMusic(music[2], true);
				break;
			case 2:
				SoundManager.Instance.playMusic(music[1], true);
				break;
			case 3:
				SoundManager.Instance.playMusic(music[0], true);
				break;
			}
		}
		else if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			SoundManager.Instance.playMusic(GameManager.Instance.wilsMusic, true);
		}
	}

	public void playOpenDoorSound()
	{
		if (doorOpens != null)
		{
			SoundManager.Instance.playSound(doorOpens);
		}
	}

	public void playCloseDoorSound()
	{
		if (doorClose != null)
		{
			SoundManager.Instance.playSound(doorClose);
		}
	}

	public abstract void activateNextRoom(ExitDirection dir);

	public void despawnNextRoom()
	{
		nextRoomController = null;
		if (nextActiveRoom != null)
		{
			Object.Destroy(nextActiveRoom.transform.root.gameObject);
		}
	}

	public void despawnCurrentRoom()
	{
		if (prevActiveRoom != null)
		{
			Object.Destroy(prevActiveRoom.transform.root.gameObject);
		}
	}

	public abstract IEnumerator autoMoveToNextRoom();

	public bool doorsAreUnlocked()
	{
		return roomController.isClear;
	}

	public abstract void setRoomCleared();

	private IEnumerator goToNextStage()
	{
		yield return new WaitForSeconds(0.5f);
		((Object.Instantiate(Resources.Load("FaderSystem")) as GameObject).GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1.5f;
		yield return new WaitForSeconds(0.2f);
		GameManager.Instance.currentStage++;
		GameManager.Instance.isGoingUpElevator = true;
		GameManager.Instance.inTutorialRoom = false;
		Application.LoadLevel("OliverCampaignLevel" + GameManager.Instance.currentStage);
	}
}
