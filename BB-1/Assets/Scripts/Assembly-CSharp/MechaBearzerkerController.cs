using System.Collections;
using AstarClasses;
using UnityEngine;

public class MechaBearzerkerController : MonoBehaviour
{
	private float maxAngle = 20f;

	private float angleCost = 2f;

	private float moveSpeed = 6f;

	private float sprintSpeed = 25f;

	private float turnSpeed = 2f;

	private float currentTurnSpeed;

	private float currentMoveSpeed;

	private AstarPath.Path currentPath;

	private ArrayList waypoints;

	private int currentWaypointIndex = -1;

	private Animation myAnimation;

	private Transform myTransform;

	private Rigidbody myRigidbody;

	private Transform currentTarget;

	public GameObject leftLeg;

	public GameObject rightLeg;

	public GameObject mainArmor;

	public GameObject upperLeftLeg;

	public GameObject upperRightLeg;

	private float maxModeChangeInterval = 6f;

	private float minModeChangeInterval = 3f;

	private float nextModeChangeInterval;

	private float lastModeChangeTime;

	private bool isSprinting;

	private bool isFlying;

	private bool isJumping;

	private Vector3 jumpTarget;

	private bool isFiringMissiles;

	private float smashSpeed = 20f;

	private float hopSpeed = 70f;

	private float jumpSpeed = 60f;

	private float rocketSpeed = 35f;

	private float rocketTime = 2f;

	private float lastRocketTime;

	private bool isLosingArmor;

	public Transform leftMachineGun;

	public Transform rightMachineGun;

	public MinigunController leftMinigun;

	public MinigunController rightMinigun;

	private bool isFiringMachineGuns;

	private float machineGunTargetSeparation = 12f;

	private float currentMachineGunTargetSeparation;

	private float machineGunTargetSpeed = 6f;

	private float machineGunLead = 16f;

	private float currentMachineGunLead;

	private Vector3 leftMachineGunTarget;

	private Vector3 currentLeftMachineGunPos;

	private Vector3 rightMachineGunTarget;

	private Vector3 currentRightMachineGunPos;

	private float collisionDamage = 35f;

	public ProjectileControllerPrefab leftMissileLauncher;

	public ProjectileControllerPrefab rightMissileLauncher;

	private Transform leftMissileLauncherTransform;

	private Transform rightMissileLauncherTransform;

	public GameObject smashPrefab;

	public GameObject backhandPrefab;

	private Collider killCollider;

	private GameObject level;

	public ParticleSystem jetpackPS;

	public ParticleSystem jetpackGroundEffectPS;

	public AudioClip missileShotSound;

	public AudioClip destroySound;

	public AudioClip jetpackLoop;

	public AudioClip walkLoop;

	public AudioClip machinegunLoop;

	public AudioClip bossMusic;

	private AudioSource audioLoop;

	public GameObject explosionEffect;

	private void Awake()
	{
		myTransform = base.transform;
		myRigidbody = GetComponent<Rigidbody>();
		leftMissileLauncherTransform = leftMissileLauncher.transform;
		rightMissileLauncherTransform = rightMissileLauncher.transform;
		audioLoop = GetComponent<AudioSource>();
	}

	private void Start()
	{
		level = myTransform.parent.gameObject;
		killCollider = myTransform.parent.Find("colliders/deathCollider").GetComponent<Collider>();
		StartCoroutine(delayedRootDisconnect());
		currentMachineGunLead = machineGunLead;
		currentMachineGunTargetSeparation = machineGunTargetSeparation;
		currentMoveSpeed = moveSpeed;
		currentTurnSpeed = turnSpeed;
		myAnimation = myTransform.Find("Mecha-Bearzerker").GetComponent<Animation>();
		myAnimation["Suit_walk"].speed = 2f;
		currentTarget = getRandomTarget();
		Component[] componentsInChildren = mainArmor.GetComponentsInChildren(typeof(DamageReceiver));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			(componentsInChildren[i] as DamageReceiver).hitpoints *= GameManager.Instance.bossHPMultipliers[GameManager.Instance.getDifficulty()];
		}
		SoundManager.Instance.playMusic(bossMusic, true);
		StartCoroutine(mainBehaviour());
	}

	private IEnumerator DelayStartMusicBoss()
	{
		yield return new WaitForSeconds(0.5f);
		SoundManager.Instance.playMusic(bossMusic, true);
	}

	private IEnumerator delayedRootDisconnect()
	{
		yield return new WaitForSeconds(0.1f);
		myTransform.parent = null;
	}

	private void OnObjectDestroyed(GameObject obj)
	{
		if (explosionEffect != null)
		{
			Object.Instantiate(explosionEffect).transform.position = obj.transform.position;
		}
		SoundManager.Instance.playSound(destroySound);
		StartCoroutine(recalculateAbilities(obj));
	}

	private IEnumerator recalculateAbilities(GameObject obj)
	{
		yield return new WaitForSeconds(0.1f);
		if (obj == mainArmor)
		{
			isLosingArmor = true;
			StopAllCoroutines();
			StartCoroutine(mainBehaviour());
		}
		if (obj == leftLeg || obj == rightLeg)
		{
			if (obj == rightLeg)
			{
				Object.Destroy(leftLeg);
			}
			else
			{
				Object.Destroy(rightLeg);
			}
			jetpackPS.enableEmission = true;
			jetpackGroundEffectPS.enableEmission = true;
			myRigidbody.useGravity = false;
			StopAllCoroutines();
			StartCoroutine(mainBehaviour());
		}
	}

	private void OnCollisionEnter(Collision c)
	{
		if (c.transform == currentTarget)
		{
			currentTarget.gameObject.SendMessage("OnHit", collisionDamage, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnCollisionStay(Collision c)
	{
		if (c.transform == currentTarget)
		{
			currentTarget.gameObject.SendMessage("OnHit", collisionDamage, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.transform == currentTarget)
		{
			currentTarget.gameObject.SendMessage("OnHit", collisionDamage, SendMessageOptions.DontRequireReceiver);
		}
		else if (c == killCollider && !isFlying)
		{
			if (mainArmor == null)
			{
				StopAllCoroutines();
				StartCoroutine(death());
			}
			else
			{
				StopAllCoroutines();
				StartCoroutine(flyOutOfPit());
			}
		}
	}

	private IEnumerator death()
	{
		yield return new WaitForSeconds(0.5f);
		((Object.Instantiate(Resources.Load("FaderSystem")) as GameObject).GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1.5f;
		yield return new WaitForSeconds(0.2f);
		if (GameManager.Instance.currentBossTrial == BossMode.NONE)
		{
			PlayerPrefs.SetString("MechabossUnlocked", "unlocked");
			GameManager.Instance.PlayMovie("o_boss1_win");
			Time.timeScale = 0f;
		}
		yield return new WaitForSeconds(0.2f);
		if (GameManager.Instance.currentGameMode != 0)
		{
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
			yield break;
		}
		int num = StatsManager.Instance.currentBearzookasPickedUp + StatsManager.Instance.currentSpreadshotsPickedUp + StatsManager.Instance.currentSatellitesPickedUp + StatsManager.Instance.currentFoodsPickedUp + StatsManager.Instance.currentShieldsPickedUp + StatsManager.Instance.currentCoffeesPickedUp + StatsManager.Instance.currentLivesPickedUp + StatsManager.Instance.currentScreenclearsPickedUp + StatsManager.Instance.currentSpecialsPickedUp;
		int pickupsMissed = StatsManager.Instance.pickupsMissed;
		StatsManager.Instance.pickupsMissed = 0;
		int wasHit = StatsManager.Instance.wasHit;
		StatsManager.Instance.wasHit = 0;
		if (GameManager.Instance.oliverUnlockedStage == 1)
		{
			GameManager.Instance.oliverUnlockedStage++;
			PlayerPrefs.SetInt("oliverUnlockedStage", GameManager.Instance.oliverUnlockedStage);
		}
		(Object.FindObjectOfType(typeof(PlayerController)) as PlayerController).saveState(Vector3.zero, Vector2.zero, "");
		GameManager.Instance.currentStage++;
		GameManager.Instance.isGoingUpElevator = true;
		GameManager.Instance.inOliverBossRoom = false;
		Application.LoadLevel("OliverCampaignLevel" + GameManager.Instance.currentStage);
	}

	private IEnumerator flyOutOfPit()
	{
		isFlying = true;
		isJumping = false;
		jumpTarget = killCollider.transform.position;
		jumpTarget.y = 0f;
		level.GetComponent<Animation>()["close"].speed = 0f;
		audioLoop.clip = jetpackLoop;
		audioLoop.volume = SoundManager.Instance.getEffectsVolume();
		audioLoop.Play();
		myAnimation["Suit_Boost"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("Suit_Boost");
		yield return new WaitForSeconds(1.5f);
		jetpackPS.enableEmission = true;
		jetpackGroundEffectPS.enableEmission = true;
		currentMoveSpeed = rocketSpeed;
		myRigidbody.useGravity = false;
		myRigidbody.velocity = Vector3.zero;
		yield return new WaitForSeconds(2.75f);
		Vector3 vector = currentTarget.position - jumpTarget;
		vector.y = 0f;
		vector.Normalize();
		jumpTarget = myTransform.position + vector * 20f;
		yield return new WaitForSeconds(2f);
		myRigidbody.useGravity = true;
		level.GetComponent<Animation>()["close"].speed = 1f;
		isFlying = false;
		jetpackPS.enableEmission = false;
		jetpackGroundEffectPS.enableEmission = false;
		StartCoroutine(mainBehaviour());
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.transform == currentTarget)
		{
			currentTarget.gameObject.SendMessage("OnHit", collisionDamage * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
		}
	}

	private IEnumerator mainBehaviour()
	{
		lastModeChangeTime = Time.time;
		while (currentTarget != null)
		{
			if (isLosingArmor)
			{
				yield return StartCoroutine(loseArmor());
			}
			else if (mainArmor == null)
			{
				float num = Vector3.Distance(myTransform.position, currentTarget.position);
				if (num <= 10f)
				{
					yield return StartCoroutine(backhand());
				}
				else if (num > 10f && num < 22f)
				{
					yield return StartCoroutine(hammerSmash());
				}
				else
				{
					yield return StartCoroutine(hop());
				}
			}
			else if (leftLeg == null || rightLeg == null)
			{
				if (Time.time < lastRocketTime + rocketTime)
				{
					yield return StartCoroutine(suitBoost(3f));
					continue;
				}
				float value = Random.value;
				if (value < 0.5f && (leftMissileLauncher != null || rightMissileLauncher != null))
				{
					yield return StartCoroutine(fireMissiles(Random.Range(2, 5), Random.value < 0.5f, false));
				}
				else if (value >= 0.5f && (rightMinigun != null || leftMinigun != null))
				{
					yield return StartCoroutine(fireMachineGuns(3f));
				}
				else
				{
					yield return StartCoroutine(suitBoost(3f));
				}
				lastRocketTime = Time.time;
			}
			else if (Time.time > lastModeChangeTime + nextModeChangeInterval)
			{
				nextModeChangeInterval = minModeChangeInterval + (maxModeChangeInterval - minModeChangeInterval) * Random.value;
				float value2 = Random.value;
				if (value2 < 0.2f)
				{
					int numJumps = Random.Range(1, 2);
					int i = 0;
					while (i < numJumps)
					{
						yield return StartCoroutine(suitJump());
						int num2 = i + 1;
						i = num2;
					}
				}
				else if (value2 >= 0.2f && value2 < 0.4f)
				{
					yield return StartCoroutine(sprint());
				}
				else if (value2 >= 0.4f && value2 < 0.6f)
				{
					yield return StartCoroutine(suitBoost(3f));
				}
				else if (value2 >= 0.6f && value2 < 0.8f && (leftMissileLauncher != null || rightMissileLauncher != null))
				{
					yield return StartCoroutine(fireMissiles(Random.Range(2, 5), Random.value < 0.5f, true));
				}
				else if (rightMinigun != null || leftMinigun != null)
				{
					yield return StartCoroutine(fireMachineGuns(3f));
				}
			}
			else if (leftLeg != null && rightLeg != null)
			{
				yield return StartCoroutine(chase());
			}
			else
			{
				yield return StartCoroutine(suitIdle(1f));
			}
		}
	}

	private IEnumerator fireMissiles(int numVolleys, bool singleMode, bool walkWhileFiring)
	{
		isFiringMissiles = true;
		if (walkWhileFiring)
		{
			currentMoveSpeed = moveSpeed;
			myAnimation["Suit_walk"].speed = 2f;
			myAnimation["Suit_walk"].wrapMode = WrapMode.Loop;
			myAnimation.CrossFade("Suit_walk");
			yield return StartCoroutine(calculatePath(currentTarget));
		}
		else
		{
			currentMoveSpeed = 0f;
			myAnimation["Suit_Idle"].wrapMode = WrapMode.Loop;
			myAnimation.CrossFade("Suit_Idle");
		}
		yield return new WaitForSeconds(1f);
		int i = 0;
		while (i < numVolleys)
		{
			if (singleMode)
			{
				if (leftMissileLauncher != null)
				{
					leftMissileLauncherTransform.LookAt(currentTarget);
					leftMissileLauncher.FireProjectile(myTransform, 1);
					SoundManager.Instance.playSound(missileShotSound);
				}
				if (walkWhileFiring)
				{
					yield return StartCoroutine(calculatePath(currentTarget));
				}
				yield return new WaitForSeconds(0.75f);
				if (rightMissileLauncher != null)
				{
					rightMissileLauncherTransform.LookAt(currentTarget);
					rightMissileLauncher.FireProjectile(myTransform, 1);
					SoundManager.Instance.playSound(missileShotSound);
				}
				if (walkWhileFiring)
				{
					yield return StartCoroutine(calculatePath(currentTarget));
				}
				yield return new WaitForSeconds(0.75f);
			}
			else
			{
				SoundManager.Instance.playSound(missileShotSound);
				if (leftMissileLauncher != null)
				{
					leftMissileLauncherTransform.LookAt(currentTarget);
					leftMissileLauncher.FireProjectile(myTransform, 1);
				}
				if (rightMissileLauncher != null)
				{
					rightMissileLauncherTransform.LookAt(currentTarget);
					rightMissileLauncher.FireProjectile(myTransform, 1);
				}
				if (walkWhileFiring)
				{
					yield return StartCoroutine(calculatePath(currentTarget));
				}
				yield return new WaitForSeconds(1.5f);
			}
			int num = i + 1;
			i = num;
		}
		isFiringMissiles = false;
	}

	private IEnumerator loseArmor()
	{
		GetComponent<Rigidbody>().useGravity = true;
		jetpackPS.enableEmission = false;
		jetpackGroundEffectPS.enableEmission = false;
		currentMoveSpeed = 0f;
		isFlying = false;
		isJumping = false;
		currentWaypointIndex = -1;
		myAnimation["Suit_Fall"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Suit_Fall");
		if (leftMinigun != null)
		{
			Object.Destroy(leftMinigun.gameObject);
		}
		if (rightMinigun != null)
		{
			Object.Destroy(rightMinigun.gameObject);
		}
		Component[] componentsInChildren = mainArmor.GetComponentsInChildren(typeof(DamageReceiver));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i]);
		}
		componentsInChildren = upperLeftLeg.GetComponentsInChildren(typeof(DamageReceiver));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i]);
		}
		componentsInChildren = upperRightLeg.GetComponentsInChildren(typeof(DamageReceiver));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i]);
		}
		yield return new WaitForSeconds(0.4f);
		GameObject gameObject = Object.Instantiate(mainArmor, mainArmor.transform.position, mainArmor.transform.rotation);
		gameObject.transform.localScale = mainArmor.transform.lossyScale;
		gameObject.GetComponent<Collider>().isTrigger = false;
		GameObject obj = Object.Instantiate(upperLeftLeg, upperLeftLeg.transform.position, upperLeftLeg.transform.rotation);
		obj.transform.localScale = upperLeftLeg.transform.lossyScale;
		Collider collider = obj.GetComponentInChildren(typeof(Collider)) as Collider;
		if (collider != null)
		{
			collider.isTrigger = false;
			Physics.IgnoreCollision(collider, GetComponent<Collider>());
		}
		GameObject obj2 = Object.Instantiate(upperRightLeg, upperRightLeg.transform.position, upperRightLeg.transform.rotation);
		obj2.transform.localScale = upperRightLeg.transform.lossyScale;
		Collider collider2 = obj2.GetComponentInChildren(typeof(Collider)) as Collider;
		if (collider2 != null)
		{
			collider2.isTrigger = false;
			Physics.IgnoreCollision(collider2, GetComponent<Collider>());
		}
		Object.Destroy(mainArmor);
		myAnimation.GetComponent<Renderer>().enabled = false;
		Object.Destroy(upperLeftLeg);
		Object.Destroy(upperRightLeg);
		componentsInChildren = currentTarget.GetComponentsInChildren(typeof(Collider));
		foreach (Component component in componentsInChildren)
		{
			Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), component as Collider);
			if (collider != null)
			{
				Physics.IgnoreCollision(collider, component as Collider);
			}
			if (collider2 != null)
			{
				Physics.IgnoreCollision(collider2, component as Collider);
			}
		}
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		yield return new WaitForSeconds(myAnimation["Suit_Fall"].length - 1.2f);
		isLosingArmor = false;
		DamageReceiver obj3 = base.gameObject.AddComponent(typeof(DamageReceiver)) as DamageReceiver;
		obj3.isInvincible = true;
		obj3.pushedByBullets = true;
		base.gameObject.layer = LayerMask.NameToLayer("Enemy");
		AirlockController airlockController = Object.FindObjectOfType<AirlockController>();
		if (airlockController != null)
		{
			airlockController.OnStartLightingAirlock();
		}
	}

	private IEnumerator backhand()
	{
		myAnimation["BackHand"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("BackHand");
		yield return new WaitForSeconds(myAnimation["BackHand"].length / 2f);
		GameObject obj = Object.Instantiate(backhandPrefab);
		Vector3 position = myTransform.position + -myTransform.forward * 5f;
		obj.transform.position = position;
		yield return new WaitForSeconds(myAnimation["BackHand"].length / 2f);
	}

	private IEnumerator hammerSmash()
	{
		jumpTarget = currentTarget.position;
		yield return StartCoroutine(idle(0.25f));
		myAnimation["Hammer_Smash"].speed = 1f;
		myAnimation["Hammer_Smash"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Hammer_Smash");
		isJumping = true;
		yield return new WaitForSeconds(0.1f);
		currentMoveSpeed = smashSpeed;
		currentWaypointIndex = -1;
		yield return new WaitForSeconds(myAnimation["Hammer_Smash"].length - 0.4f);
		myAnimation["Hammer_Smash"].speed = 0f;
		currentMoveSpeed = 0f;
		GameObject obj = Object.Instantiate(smashPrefab);
		Vector3 position = myTransform.position + -myTransform.forward * 7f;
		obj.transform.position = position;
		yield return new WaitForSeconds(1f);
		currentTurnSpeed = 20f;
		isJumping = false;
		yield return StartCoroutine(idle(1f));
		lastModeChangeTime = Time.time;
		currentTurnSpeed = turnSpeed;
	}

	private IEnumerator hop()
	{
		jumpTarget = currentTarget.position;
		audioLoop.Stop();
		myAnimation["Hop"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Hop");
		isJumping = true;
		yield return new WaitForSeconds(0.1f);
		currentMoveSpeed = hopSpeed;
		currentWaypointIndex = -1;
		yield return new WaitForSeconds(myAnimation["Hop"].length - 0.75f);
		currentMoveSpeed = 0f;
		currentTurnSpeed = 20f;
		lastModeChangeTime = Time.time;
		isJumping = false;
		yield return new WaitForSeconds(0.75f);
		currentTurnSpeed = turnSpeed;
	}

	private IEnumerator idle(float idleTime)
	{
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		myAnimation["idle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("idle");
		yield return new WaitForSeconds(idleTime);
	}

	private IEnumerator sprint()
	{
		currentMoveSpeed = sprintSpeed;
		myAnimation["Suit_walk"].speed = 6f;
		myAnimation["Suit_walk"].wrapMode = WrapMode.Loop;
		lastModeChangeTime = Time.time;
		isSprinting = true;
		audioLoop.Pause();
		int i = 0;
		while (i < 3)
		{
			jumpTarget = currentTarget.position;
			yield return new WaitForSeconds(0.4f);
			int num = i + 1;
			i = num;
		}
		isSprinting = false;
		myAnimation["Suit_walk"].speed = 2f;
		currentMoveSpeed = moveSpeed;
	}

	private IEnumerator chase()
	{
		currentMoveSpeed = moveSpeed;
		audioLoop.clip = walkLoop;
		audioLoop.volume = SoundManager.Instance.getEffectsVolume();
		audioLoop.Play();
		myAnimation["Suit_walk"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("Suit_walk");
		yield return StartCoroutine(calculatePath(currentTarget));
		yield return new WaitForSeconds(1f);
	}

	private IEnumerator fireMachineGuns(float shootTime)
	{
		isFiringMachineGuns = true;
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		audioLoop.clip = machinegunLoop;
		audioLoop.volume = SoundManager.Instance.getEffectsVolume();
		audioLoop.Play();
		myAnimation["Suit_Gunspray"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Suit_Gunspray");
		yield return new WaitForSeconds(0.5f);
		if (leftMinigun != null)
		{
			leftMinigun.startFiring();
		}
		if (rightMinigun != null)
		{
			rightMinigun.startFiring();
		}
		currentLeftMachineGunPos = leftMachineGunTarget;
		currentRightMachineGunPos = rightMachineGunTarget;
		yield return new WaitForSeconds(shootTime - 0.5f);
		if (leftMinigun != null)
		{
			leftMinigun.stopFiring();
		}
		if (rightMinigun != null)
		{
			rightMinigun.stopFiring();
		}
		yield return new WaitForSeconds(Mathf.Max(0f, myAnimation["Suit_Gunspray"].length - shootTime));
		lastModeChangeTime = Time.time;
		currentMachineGunTargetSeparation = machineGunTargetSeparation;
		currentMachineGunLead = machineGunLead;
		isFiringMachineGuns = false;
	}

	private IEnumerator suitIdle(float idleTime)
	{
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		myAnimation["Suit_Idle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("Suit_Idle");
		yield return new WaitForSeconds(idleTime);
	}

	private IEnumerator suitJump()
	{
		jumpTarget = currentTarget.position;
		myAnimation["Suit_Jump"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Suit_Jump");
		isJumping = true;
		yield return new WaitForSeconds(0.3f);
		currentMoveSpeed = jumpSpeed;
		currentWaypointIndex = -1;
		yield return new WaitForSeconds(myAnimation["Suit_Jump"].length - 0.3f - 0.2f);
		currentMoveSpeed = 0f;
		currentTurnSpeed = 20f;
		lastModeChangeTime = Time.time;
		isJumping = false;
		yield return new WaitForSeconds(0.4f);
		currentTurnSpeed = turnSpeed;
	}

	private IEnumerator suitBoost(float flyTime)
	{
		currentMoveSpeed = rocketSpeed;
		myRigidbody.useGravity = false;
		jetpackPS.enableEmission = true;
		jetpackGroundEffectPS.enableEmission = true;
		isFlying = true;
		audioLoop.clip = jetpackLoop;
		audioLoop.volume = SoundManager.Instance.getEffectsVolume();
		audioLoop.Play();
		myAnimation["Suit_Boost"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("Suit_Boost");
		jumpTarget = currentTarget.position;
		yield return new WaitForSeconds(flyTime);
		lastModeChangeTime = Time.time;
		isFlying = false;
		if (leftLeg != null && rightLeg != null)
		{
			jetpackPS.enableEmission = false;
			jetpackGroundEffectPS.enableEmission = false;
			myRigidbody.useGravity = true;
		}
	}

	private IEnumerator calculatePath(Transform target)
	{
		yield return StartCoroutine(StartPath(new Vector3(myTransform.position.x, 0f, myTransform.position.z), new Vector3(target.position.x, 0f, target.position.z)));
		if (currentPath.path != null)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 1; i < currentPath.path.Length; i++)
			{
				arrayList.Add(currentPath.path[i].vectorPos);
			}
			waypoints = arrayList;
			currentWaypointIndex = 0;
		}
		yield return new WaitForSeconds(0.7f);
	}

	private IEnumerator StartPath(Vector3 start, Vector3 end)
	{
		AstarPath.Path p = new AstarPath.Path(start, end, maxAngle, angleCost, false);
		yield return StartCoroutine(AstarPath.StartPathYield(p));
		currentPath = p;
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
		if (isFiringMachineGuns)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(leftMachineGunTarget, 1f);
			Gizmos.DrawSphere(rightMachineGunTarget, 1f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(currentLeftMachineGunPos, 0.75f);
			Gizmos.DrawSphere(currentRightMachineGunPos, 0.75f);
		}
	}

	private bool moveTowards(Vector3 targetPos, bool useY)
	{
		Vector3 position = myTransform.position;
		Vector3 vector = targetPos - position;
		if (!useY)
		{
			vector.y = 0f;
		}
		if (vector.magnitude * 1.5f > currentMoveSpeed * Time.deltaTime)
		{
			vector.Normalize();
			position += vector * currentMoveSpeed * Time.deltaTime;
			myTransform.position = position;
			return false;
		}
		return true;
	}

	private void rotateTowards(Vector3 position)
	{
		Vector3 forward = myTransform.position - position;
		forward.y = 0f;
		if (!(forward.magnitude < 0.1f))
		{
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(forward), currentTurnSpeed * Time.deltaTime);
			myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
		}
	}

	private void LateUpdate()
	{
		Vector3 velocity = myRigidbody.velocity;
		velocity.x = 0f;
		velocity.z = 0f;
		if (velocity.y > 0f)
		{
			velocity.y = 0f;
		}
		myRigidbody.velocity = velocity;
		if (currentTarget == null)
		{
			return;
		}
		if (isFlying || isSprinting)
		{
			rotateTowards(currentTarget.position);
			moveTowards(jumpTarget, isFlying);
		}
		else if (isJumping)
		{
			moveTowards(jumpTarget, false);
		}
		else if (isFiringMissiles)
		{
			if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count && currentPath.path != null && moveTowards((Vector3)waypoints[currentWaypointIndex], false))
			{
				currentWaypointIndex++;
			}
			rotateTowards(currentTarget.position);
		}
		else if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count && currentPath.path != null)
		{
			rotateTowards((Vector3)waypoints[currentWaypointIndex]);
			if (moveTowards((Vector3)waypoints[currentWaypointIndex], false))
			{
				currentWaypointIndex++;
			}
		}
		else
		{
			rotateTowards(currentTarget.position);
		}
		if (!isFiringMachineGuns)
		{
			return;
		}
		currentMachineGunTargetSeparation -= Time.deltaTime * machineGunTargetSpeed;
		currentMachineGunLead -= Time.deltaTime * machineGunTargetSpeed * 1.5f;
		if (leftMachineGun != null)
		{
			Vector3 vector = currentTarget.position - leftMachineGun.position;
			vector.Normalize();
			Vector3 vector2 = -vector;
			float x = vector.x;
			vector.x = vector.z;
			vector.z = 0f - x;
			leftMachineGunTarget = currentTarget.position + -vector * currentMachineGunTargetSeparation + vector2 * currentMachineGunLead;
			leftMachineGunTarget.y = 0f;
			float num = 12f;
			Vector3 vector3 = leftMachineGunTarget - currentLeftMachineGunPos;
			if (vector3.magnitude < num * Time.deltaTime)
			{
				currentLeftMachineGunPos = leftMachineGunTarget;
			}
			else
			{
				vector3.Normalize();
				currentLeftMachineGunPos += vector3 * Time.deltaTime * num;
			}
			leftMachineGun.LookAt(currentLeftMachineGunPos);
			Vector3 localEulerAngles = leftMachineGun.transform.localEulerAngles;
			localEulerAngles.y += 180f;
			localEulerAngles.x = 0f - localEulerAngles.x;
			if (localEulerAngles.y > 30f && localEulerAngles.y < 180f)
			{
				localEulerAngles.y = 30f;
			}
			else if (localEulerAngles.y < 330f && localEulerAngles.y > 180f)
			{
				localEulerAngles.y = 330f;
			}
			if (localEulerAngles.x < -25f)
			{
				localEulerAngles.x = -25f;
			}
			leftMachineGun.transform.localEulerAngles = localEulerAngles;
		}
		if (rightMachineGun != null)
		{
			Vector3 vector4 = currentTarget.position - rightMachineGun.position;
			vector4.Normalize();
			Vector3 vector5 = -vector4;
			float x2 = vector4.x;
			vector4.x = vector4.z;
			vector4.z = 0f - x2;
			rightMachineGunTarget = currentTarget.position + vector4 * currentMachineGunTargetSeparation + vector5 * currentMachineGunLead;
			rightMachineGunTarget.y = 0f;
			float num2 = 12f;
			Vector3 vector6 = rightMachineGunTarget - currentRightMachineGunPos;
			if (vector6.magnitude < num2 * Time.deltaTime)
			{
				currentRightMachineGunPos = rightMachineGunTarget;
			}
			else
			{
				vector6.Normalize();
				currentRightMachineGunPos += vector6 * Time.deltaTime * num2;
			}
			rightMachineGun.LookAt(currentRightMachineGunPos);
			Vector3 localEulerAngles2 = rightMachineGun.transform.localEulerAngles;
			localEulerAngles2.y += 180f;
			localEulerAngles2.x = 0f - localEulerAngles2.x;
			if (localEulerAngles2.y > 30f && localEulerAngles2.y < 180f)
			{
				localEulerAngles2.y = 30f;
			}
			else if (localEulerAngles2.y < 330f && localEulerAngles2.y > 180f)
			{
				localEulerAngles2.y = 330f;
			}
			if (localEulerAngles2.x < -25f)
			{
				localEulerAngles2.x = -25f;
			}
			rightMachineGun.transform.localEulerAngles = localEulerAngles2;
		}
	}

	private Transform getRandomTarget()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		if (array.Length != 0)
		{
			return array[Random.Range(0, array.Length)].transform;
		}
		return null;
	}
}
