using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
	private GameObject transitionCollider;

	private Transform spawnPoint;

	public ExitDirection exitDirection;

	private Transform myTransform;

	public static bool inTransition;

	private bool isOpen;

	public DoorHitZone hitZone;

	public GameObject blackBackground;

	public GameObject greenLight;

	public GameObject yellowLight;

	public GameObject redLight;

	private Animation doorAnimation;

	private Transform transitionPoint;

	private bool playOpenSound = true;

	private bool isSpawnOpen;

	private float spawnOpenTimeLeft;

	private float closeDistance;

	public void setPlayOpenSound(bool toggle)
	{
		playOpenSound = toggle;
	}

	private void Awake()
	{
		myTransform = base.transform;
		spawnPoint = myTransform.Find("SpawnPoint");
		transitionCollider = myTransform.Find("Transition/Collider").gameObject;
		transitionPoint = myTransform.Find("Transition");
		doorAnimation = GetComponent<Animation>();
		redLight.SetActiveRecursively(false);
		yellowLight.SetActiveRecursively(false);
		greenLight.SetActiveRecursively(true);
		exitDirection = (ExitDirection)(Mathf.RoundToInt(myTransform.localEulerAngles.y) / 90);
		if (GameManager.Instance.currentCharacter == Character.WIL && GameManager.Instance.currentStage == 1)
		{
			closeDistance = 30f;
		}
		else
		{
			closeDistance = 10f;
		}
	}

	public Transform getTransitionPoint()
	{
		return transitionPoint;
	}

	public Transform getSpawnPoint()
	{
		return spawnPoint;
	}

	private void Start()
	{
		isOpen = false;
		blackBackground.active = false;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!(other.tag != "Player") && isOpen)
		{
			StartCoroutine(ModeManager.Instance.autoMoveToNextRoom());
		}
	}

	public void openDoor(Transform playerTransform)
	{
		if (!isOpen && !(ModeManager.Instance == null))
		{
			if (playOpenSound)
			{
				ModeManager.Instance.playOpenDoorSound();
			}
			else
			{
				playOpenSound = true;
			}
			StartCoroutine(openDoorAnimation());
			isOpen = true;
			ModeManager.Instance.activateNextRoom(exitDirection);
			StartCoroutine(startTimer(playerTransform));
		}
	}

	private IEnumerator startTimer(Transform playerTransform)
	{
		do
		{
			yield return new WaitForSeconds(1f);
		}
		while (Vector3.Distance(playerTransform.position, myTransform.position) < closeDistance);
		StartCoroutine(closeDoor(playerTransform));
	}

	private IEnumerator closeDoor(Transform playerTransform)
	{
		ModeManager.Instance.playCloseDoorSound();
		yield return StartCoroutine(closeDoorAnimation());
		ModeManager.Instance.despawnNextRoom();
		isOpen = false;
		if (Vector3.Distance(playerTransform.position, myTransform.position) < 10f)
		{
			openDoor(playerTransform);
		}
	}

	private IEnumerator closeDoorAnimation()
	{
		doorAnimation["open"].speed = -1f;
		doorAnimation["open"].normalizedTime = 1f;
		doorAnimation.Play("open");
		yield return new WaitForSeconds(doorAnimation["open"].length);
	}

	private IEnumerator openDoorAnimation()
	{
		doorAnimation["open"].normalizedTime = 0f;
		doorAnimation["open"].speed = 1f;
		doorAnimation.Play("open");
		yield return new WaitForSeconds(doorAnimation["open"].length);
	}

	public void disableDoor()
	{
		turnOnRedLight();
	}

	public void enableDoor()
	{
		turnOnGreenLight();
	}

	public void turnOnGreenLight()
	{
		redLight.SetActiveRecursively(false);
		yellowLight.SetActiveRecursively(false);
		greenLight.SetActiveRecursively(true);
		hitZone.gameObject.active = true;
		transitionCollider.active = true;
	}

	public void turnOnRedLight()
	{
		redLight.SetActiveRecursively(true);
		yellowLight.SetActiveRecursively(false);
		greenLight.SetActiveRecursively(false);
		hitZone.gameObject.active = false;
		transitionCollider.active = false;
	}

	public void turnOnYellowLight()
	{
		redLight.SetActiveRecursively(false);
		yellowLight.SetActiveRecursively(true);
		greenLight.SetActiveRecursively(false);
	}

	public IEnumerator spawningOpen()
	{
		if (!isSpawnOpen)
		{
			blackBackground.active = true;
			turnOnYellowLight();
			isSpawnOpen = true;
			StartCoroutine(spawningCloseTimer());
			yield return StartCoroutine(openDoorAnimation());
		}
		else
		{
			spawnOpenTimeLeft = 5f;
		}
	}

	public IEnumerator spawningCloseTimer()
	{
		for (spawnOpenTimeLeft = 5f; spawnOpenTimeLeft > 0f; spawnOpenTimeLeft -= 0.5f)
		{
			yield return new WaitForSeconds(0.5f);
		}
		isSpawnOpen = false;
		turnOnRedLight();
		blackBackground.active = false;
		yield return StartCoroutine(closeDoorAnimation());
	}

	public void OnClose()
	{
		if (isSpawnOpen)
		{
			isSpawnOpen = false;
			StartCoroutine(closeDoorAnimation());
			turnOnRedLight();
			blackBackground.active = false;
		}
	}
}
