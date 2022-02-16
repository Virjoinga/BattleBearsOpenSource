using System.Collections;
using AstarClasses;
using UnityEngine;

public class DetachedUdderController : MonoBehaviour
{
	public Material whiteMaterial;

	private Material originalMaterial;

	public Renderer myRenderer;

	public float maxHP = 350f;

	private float currentHP;

	private bool isWhite;

	private Animation myAnimation;

	private Transform myTransform;

	private float damageSoFar;

	public float stunEveryXDamage = 100f;

	private float maxAngle = 20f;

	private float angleCost = 2f;

	public float moveSpeed = 3f;

	public float turnSpeed = 5f;

	private float currentTurnSpeed;

	private float currentMoveSpeed;

	private AstarPath.Path currentPath;

	private ArrayList waypoints = new ArrayList();

	private int currentWaypointIndex = -1;

	private Rigidbody myRigidbody;

	private Transform currentTarget;

	private Vector3 currentLookTarget;

	private Transform[] runTargets;

	public ParticleSystem milkEmitterPS;

	public GameObject inkCloud;

	public AudioClip hitSound;

	public AudioClip spinSound;

	public AudioClip inkSound;

	private AudioSource myAudio;

	private bool hasRecentlyMadeSound;

	private void Awake()
	{
		currentMoveSpeed = moveSpeed;
		currentTurnSpeed = turnSpeed;
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = base.transform;
		myAnimation = GetComponent<Animation>();
		myAudio = GetComponent<AudioSource>();
		myAudio.volume = SoundManager.Instance.getEffectsVolume();
		maxHP *= GameManager.Instance.bossHPMultipliers[GameManager.Instance.getDifficulty()];
		currentHP = maxHP;
		if (myRenderer == null && GetComponent<Renderer>() != null)
		{
			myRenderer = GetComponent<Renderer>();
		}
		if (myRenderer != null)
		{
			originalMaterial = myRenderer.sharedMaterial;
		}
		ParticleSystem.EmissionModule emission = milkEmitterPS.emission;
		emission.enabled = false;
	}

	private void Start()
	{
		Transform transform = myTransform.root.Find("runTargets");
		runTargets = new Transform[transform.childCount];
		int num = 0;
		foreach (Transform item in transform)
		{
			runTargets[num++] = item;
		}
		getRandomTarget();
		myAnimation["UdderRun"].wrapMode = WrapMode.Loop;
		myAnimation["UdderRun"].speed = 4f;
		myAnimation.CrossFade("UdderRun");
		StartCoroutine(calculatePath(currentTarget));
		StartCoroutine(mover());
		StartCoroutine("mainBehaviour");
	}

	private IEnumerator temporarySoundDisable()
	{
		hasRecentlyMadeSound = true;
		yield return new WaitForSeconds(1f);
		hasRecentlyMadeSound = false;
	}

	private IEnumerator mainBehaviour()
	{
		while (true)
		{
			if (GameManager.Instance.currentDifficulty == GameDifficulty.EASY)
			{
				yield return new WaitForSeconds(Random.value * 10f + 8f);
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.MEDIUM)
			{
				yield return new WaitForSeconds(Random.value * 8f + 6f);
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.HARD)
			{
				yield return new WaitForSeconds(Random.value * 5f + 2f);
			}
			if (Random.value < 0.3f)
			{
				yield return StartCoroutine(jumpSpin());
			}
			else
			{
				yield return StartCoroutine(inkCloudSpawn());
			}
			myAnimation["UdderRun"].wrapMode = WrapMode.Loop;
			myAnimation["UdderRun"].speed = 4f;
			myAnimation.CrossFade("UdderRun");
		}
	}

	private IEnumerator inkCloudSpawn()
	{
		myRigidbody.velocity = Vector3.zero;
		currentMoveSpeed = 0f;
		myAnimation["UdderHideIdle"].wrapMode = WrapMode.Loop;
		myAnimation["UdderHideIdle"].speed = 1f;
		myAnimation.CrossFade("UdderHideIdle");
		myAudio.Stop();
		yield return new WaitForSeconds(0.25f);
		SoundManager.Instance.playSound(inkSound);
		Object.Instantiate(inkCloud).transform.position = myTransform.position;
		yield return new WaitForSeconds(2f);
		myAudio.Play();
		currentMoveSpeed = moveSpeed;
	}

	private IEnumerator jumpSpin()
	{
		currentMoveSpeed = 12f;
		myAnimation["UdderDetachAtk"].wrapMode = WrapMode.Once;
		myAnimation["UdderDetachAtk"].speed = 1f;
		myAnimation.CrossFade("UdderDetachAtk");
		yield return new WaitForSeconds(0.5f);
		ParticleSystem.EmissionModule emit = milkEmitterPS.emission;
		emit.enabled = true;
		milkEmitterPS.Play();
		myAudio.Stop();
		SoundManager.Instance.playSound(spinSound);
		yield return new WaitForSeconds(1f);
		myAudio.Play();
		emit.enabled = false;
		milkEmitterPS.Stop();
		yield return new WaitForSeconds(myAnimation["UdderDetachAtk"].length - 1.5f);
		currentMoveSpeed = moveSpeed;
	}

	private void getRandomTarget()
	{
		int num = Random.Range(0, runTargets.Length);
		if (runTargets[num] == currentTarget)
		{
			num = (num + 1) % runTargets.Length;
			currentTarget = runTargets[num];
		}
		else
		{
			currentTarget = runTargets[num];
		}
	}

	private IEnumerator turnWhite()
	{
		isWhite = true;
		myRenderer.sharedMaterial = whiteMaterial;
		yield return new WaitForSeconds(0.2f);
		myRenderer.sharedMaterial = originalMaterial;
		isWhite = false;
	}

	public void OnUdderHit(float damage)
	{
		if (!(currentHP <= 0f))
		{
			if (!hasRecentlyMadeSound)
			{
				SoundManager.Instance.playSound(hitSound);
				StartCoroutine(temporarySoundDisable());
			}
			if (!isWhite)
			{
				StartCoroutine(turnWhite());
			}
			currentHP -= damage;
			damageSoFar += damage;
			if (currentHP <= 0f)
			{
				StopAllCoroutines();
				StartCoroutine(OnDeath());
			}
			else if (damageSoFar > stunEveryXDamage)
			{
				damageSoFar = 0f;
				StartCoroutine(stun());
			}
		}
	}

	private IEnumerator hide(float hideTime)
	{
		currentMoveSpeed = 0f;
		myAnimation["UdderHideIdle"].wrapMode = WrapMode.Loop;
		myAnimation["UdderHideIdle"].speed = 1f;
		myAnimation.CrossFade("UdderHideIdle");
		yield return new WaitForSeconds(hideTime);
		myAnimation["UdderRun"].wrapMode = WrapMode.Loop;
		myAnimation["UdderRun"].speed = 4f;
		myAnimation.CrossFade("UdderRun");
		currentMoveSpeed = moveSpeed;
	}

	private IEnumerator stun()
	{
		StopCoroutine("mainBehaviour");
		ParticleSystem.EmissionModule emission = milkEmitterPS.emission;
		emission.enabled = false;
		currentMoveSpeed = 0f;
		myAnimation["UdderDamageGetUp"].wrapMode = WrapMode.Once;
		myAnimation["UdderDamageGetUp"].speed = 1f;
		myAnimation.CrossFade("UdderDamageGetUp");
		yield return new WaitForSeconds(myAnimation["UdderDamageGetUp"].length);
		currentMoveSpeed = moveSpeed;
		myAnimation["UdderRun"].wrapMode = WrapMode.Loop;
		myAnimation["UdderRun"].speed = 4f;
		myAnimation.CrossFade("UdderRun");
		StartCoroutine("mainBehaviour");
	}

	private IEnumerator OnDeath()
	{
		StatsManager.Instance.currentPlayTime += Time.timeSinceLevelLoad;
		if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
		{
			StatsManager.Instance.getPlaytimeString();
			switch (GameManager.Instance.currentDifficulty)
			{
			case GameDifficulty.MEDIUM:
				if (GameManager.Instance.startingStage != 1)
				{
				}
				break;
			case GameDifficulty.HARD:
			{
				int startingStage = GameManager.Instance.startingStage;
				int num = 1;
				break;
			}
			}
			int num2 = StatsManager.Instance.currentBearzookasPickedUp + StatsManager.Instance.currentSpreadshotsPickedUp + StatsManager.Instance.currentSatellitesPickedUp + StatsManager.Instance.currentFoodsPickedUp + StatsManager.Instance.currentShieldsPickedUp + StatsManager.Instance.currentCoffeesPickedUp + StatsManager.Instance.currentLivesPickedUp + StatsManager.Instance.currentScreenclearsPickedUp + StatsManager.Instance.currentSpecialsPickedUp;
			int pickupsMissed = StatsManager.Instance.pickupsMissed;
			float currentPlayTime = StatsManager.Instance.currentPlayTime;
			float num3 = 1200f;
			int wasHit = StatsManager.Instance.wasHit;
			float num4 = (float)(StatsManager.Instance.currentPinksKilled + StatsManager.Instance.currentGreensKilled + StatsManager.Instance.currentOrangesKilled + StatsManager.Instance.currentProjectilesKilled + StatsManager.Instance.currentYellowsKilled + StatsManager.Instance.currentRedsKilled + StatsManager.Instance.currentBluesKilled + StatsManager.Instance.currentTurretsKilled + StatsManager.Instance.currentSecretsKilled) / (float)StatsManager.Instance.currentShotsFired;
			float num5 = 0.1f;
			float num6 = 0.25f;
			float num7 = 0.5f;
			float num8 = 1f;
		}
		PlayerController playerController = Object.FindObjectOfType(typeof(PlayerController)) as PlayerController;
		if ((playerController.gameObject.GetComponentInChildren(typeof(DamageReceiver)) as DamageReceiver).hitpoints / (float)playerController.maxHealth <= 0.05f)
		{
			BossMode currentBossTrial = GameManager.Instance.currentBossTrial;
			int num9 = 3;
		}
		ParticleSystem.EmissionModule emission = milkEmitterPS.emission;
		emission.enabled = false;
		currentMoveSpeed = 0f;
		myAnimation["UdderDamageGetUp"].wrapMode = WrapMode.Once;
		myAnimation["UdderDamageGetUp"].speed = 1f;
		myAnimation.CrossFade("UdderDamageGetUp");
		yield return new WaitForSeconds(0.5f);
		myAnimation["UdderDamageGetUp"].speed = 0f;
		myRigidbody.velocity = new Vector3(0f, -1f, 0f);
		if (GameManager.Instance.currentGameMode != 0)
		{
			yield return new WaitForSeconds(4f);
		}
		Camera[] allCameras = Camera.allCameras;
		foreach (Camera camera in allCameras)
		{
			if (camera.gameObject.name != "clearCamera")
			{
				camera.enabled = false;
			}
		}
		if (GameManager.Instance.currentBossTrial == BossMode.NONE)
		{
			PlayerPrefs.DeleteKey("character");
			PlayerPrefs.SetString("TentacleeseUnlocked", "unlocked");
		}
		if (GameManager.Instance.currentBossTrial == BossMode.NONE)
		{
			if (GameManager.Instance.riggsUnlockedStage == 1)
			{
				GameManager.Instance.riggsUnlockedStage++;
				PlayerPrefs.SetInt("riggsUnlockedStage", GameManager.Instance.riggsUnlockedStage);
			}
			GameManager.Instance.PlayMovie("r_boss1udder_win");
		}
		yield return new WaitForSeconds(0.2f);
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (!(gameObject.name == "GameManager") && !(gameObject.transform.root.name == "SoundManager") && !(gameObject.name == "Controlls") && !(gameObject.name == "Rewired Input Manager") && !(gameObject.name == "UICanvas") && !(gameObject.name == "EventSystem"))
			{
				Object.Destroy(gameObject);
			}
		}
		Object.Instantiate((Object.FindObjectOfType(typeof(EndMenuSpawner)) as EndMenuSpawner).endMenu);
	}

	private IEnumerator calculatePath(Transform target)
	{
		StartCoroutine(StartPath(new Vector3(myTransform.position.x, 0f, myTransform.position.z), new Vector3(target.position.x, 0f, target.position.z)));
		while (currentPath == null || currentPath.path == null || (!currentPath.error && !currentPath.foundEnd))
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (currentPath.foundEnd)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 1; i < currentPath.path.Length; i++)
			{
				arrayList.Add(currentPath.path[i].vectorPos);
			}
			waypoints = arrayList;
			currentWaypointIndex = 0;
		}
		else
		{
			myRigidbody.velocity = Vector3.zero;
			currentWaypointIndex = -1;
		}
	}

	private IEnumerator StartPath(Vector3 start, Vector3 end)
	{
		yield return StartCoroutine(AstarPath.StartPath(currentPath = new AstarPath.Path(start, end, maxAngle, angleCost, false)));
	}

	private bool moveTowards(Vector3 targetPos, float timeElapsed)
	{
		Vector3 vector = targetPos - myTransform.position;
		vector.y = 0f;
		if (vector.magnitude < currentMoveSpeed * timeElapsed * 1.2f)
		{
			myTransform.position = targetPos;
			myRigidbody.velocity = Vector3.zero;
			currentLookTarget = currentTarget.position;
			return true;
		}
		vector.Normalize();
		myRigidbody.velocity = vector * currentMoveSpeed;
		return false;
	}

	private void OnDrawGizmos()
	{
		if (currentPath != null && currentPath.path != null)
		{
			Gizmos.color = Color.green;
			for (int i = 0; i < currentPath.path.Length - 1; i++)
			{
				Node obj = currentPath.path[i];
				Node node = currentPath.path[i + 1];
				Gizmos.DrawSphere(obj.vectorPos, 1f);
				Gizmos.DrawLine(obj.vectorPos, node.vectorPos);
			}
		}
	}

	private void rotateTowards(Vector3 position, float timeElapsed)
	{
		Vector3 forward = myTransform.position - position;
		forward.y = 0f;
		if (!(forward.magnitude < 0.1f))
		{
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(forward), currentTurnSpeed * timeElapsed);
			myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
		}
	}

	private IEnumerator mover()
	{
		while (true)
		{
			myTransform.position = new Vector3(myTransform.position.x, 0f, myTransform.position.z);
			if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
			{
				rotateTowards((Vector3)waypoints[currentWaypointIndex], 0.05f);
				if (moveTowards((Vector3)waypoints[currentWaypointIndex], 0.05f))
				{
					currentWaypointIndex++;
				}
			}
			else
			{
				currentWaypointIndex = -1;
				getRandomTarget();
				yield return StartCoroutine(calculatePath(currentTarget));
			}
			yield return new WaitForSeconds(0.05f);
		}
	}
}
