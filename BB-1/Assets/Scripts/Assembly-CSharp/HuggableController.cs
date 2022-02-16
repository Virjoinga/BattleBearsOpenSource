using System.Collections;
using AstarClasses;
using UnityEngine;

public class HuggableController : MonoBehaviour
{
	private float pathDelay = 1f;

	public int scoreValue = 1000;

	private float LODdistance = 25f;

	protected float maxAngle = 90f;

	protected float angleCost = 200f;

	public float moveSpeed = 3f;

	public float turnSpeed = 5f;

	protected float currentTurnSpeed;

	protected float currentMoveSpeed;

	protected AstarPath.Path currentPath;

	protected ArrayList waypoints = new ArrayList();

	protected int currentWaypointIndex = -1;

	protected Animation myAnimation;

	protected Transform myTransform;

	protected Transform currentTarget;

	protected bool isDying;

	public float minIdleTime = 1f;

	public float maxIdleTime = 3f;

	public float collisionDamage = 20f;

	protected float lastIdleTime;

	protected float idleDelay = 5f;

	public GameObject meleeAttack;

	public GameObject rangedAttack;

	public float rangedFiringForce = 10f;

	public LayerMask chaseMask;

	public LayerMask shootMask;

	protected Transform attackSpawnpoint;

	public bool explodeOnAttack;

	public float moveAnimationSpeedDivider = 1f;

	public float attackAnimationSpeedMultiplier = 1f;

	public float attackDistance = 3f;

	public GameObject[] deathDrops;

	public float[] dropProbabilities;

	public GameObject[] treasureDrops;

	public float[] treasureDropProbabilities;

	public int treasureToDrop = 1;

	public bool spawnAllSameType;

	public int numberToDrop = 1;

	protected Rigidbody myRigidbody;

	protected Collider myCollider;

	protected SkinnedMeshRenderer myRenderer;

	public static ArrayList currentBaddies = new ArrayList();

	public Mesh LODmesh;

	protected Mesh highQualityMesh;

	protected bool isLOD;

	protected Vector3 currentLookTarget;

	public string[] deathAnimations;

	public GameObject explodeSplatterEffect;

	public float explodeHeight = 1f;

	public GameObject deathEffect;

	protected bool explodeOnDeath;

	protected bool headChopped;

	public AudioClip deathSound;

	public AudioClip[] attackSounds;

	public AudioClip explodeSound;

	public AudioClip squishSound;

	public AudioClip headshotSound;

	protected bool hasEnteredPlay;

	public string entryAnimation = "HugWalkA";

	public float entryMoveSpeed = 3f;

	protected DamageReceiver dmgReceiver;

	public bool isRanged;

	public GameObject headshotRainbow;

	protected bool hasRemovedSelf;

	protected GameObject currentMeleeAttack;

	public string[] attackAnimations;

	public string idleAnimation = "";

	public string walkAnimation = "";

	private float _delayCheckNotStart = 3f;

	public string moveLog = "";

	public void OnSetStartPoint(Vector3 startPoint)
	{
		waypoints.Clear();
		waypoints.Add(new Vector3(startPoint.x, 0f, startPoint.z));
		currentWaypointIndex = 0;
		currentLookTarget = startPoint;
	}

	private void Awake()
	{
		if (attackAnimations.Length == 0)
		{
			attackAnimations = new string[3];
			attackAnimations[0] = "HugAttackA";
			attackAnimations[1] = "HugAttackB";
			attackAnimations[2] = "HugAttackC";
		}
		if (idleAnimation == "")
		{
			idleAnimation = "HugIdleA";
		}
		if (walkAnimation == "")
		{
			walkAnimation = "HugWalkA";
		}
		if (GameManager.Instance.isIpad)
		{
			LODdistance = 250f;
		}
		else if (GameManager.Instance.useHighres)
		{
			LODdistance = 50f;
		}
		dmgReceiver = GetComponentInChildren(typeof(DamageReceiver)) as DamageReceiver;
		dmgReceiver.isInvincible = true;
		myCollider = GetComponent<Collider>();
	}

	protected virtual void Start()
	{
		for (int i = 0; i < currentBaddies.Count; i++)
		{
			Physics.IgnoreCollision(currentBaddies[i] as Collider, myCollider);
		}
		for (int j = 0; j < RoomTile.currentRoomColliders.Count; j++)
		{
			Collider collider = RoomTile.currentRoomColliders[j] as Collider;
			if (collider != null && collider.gameObject.active)
			{
				Physics.IgnoreCollision(collider, myCollider);
			}
		}
		for (int k = 0; k < PickupController.currentPickups.Count; k++)
		{
			Physics.IgnoreCollision(PickupController.currentPickups[k] as Collider, myCollider);
		}
		currentBaddies.Add(myCollider);
		myRigidbody = GetComponent<Rigidbody>();
		currentMoveSpeed = moveSpeed;
		currentTurnSpeed = turnSpeed;
		myTransform = base.transform;
		myAnimation = GetComponentInChildren(typeof(Animation)) as Animation;
		myRenderer = GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
		if (myRenderer != null)
		{
			highQualityMesh = myRenderer.sharedMesh;
		}
		attackSpawnpoint = myTransform.Find("attackSpawnpoint");
		currentTarget = getRandomTarget();
		switchLOD();
		lastIdleTime = Time.time;
		currentMoveSpeed = entryMoveSpeed;
		myAnimation[entryAnimation].wrapMode = WrapMode.Loop;
		myAnimation[entryAnimation].speed = 1f;
		myAnimation.CrossFade(entryAnimation);
		StartCoroutine(mover());
		StartCoroutine(lodChecker());
	}

	private void OnActivate()
	{
		hasEnteredPlay = true;
		dmgReceiver.isInvincible = false;
		StartCoroutine(mainBehaviour());
	}

	private void OnFinalDeath()
	{
		if (EnemySpawner.Instance != null)
		{
			EnemySpawner.Instance.removeEnemy(base.gameObject);
		}
	}

	private void OnDisable()
	{
		if (!hasRemovedSelf)
		{
			currentBaddies.Remove(myCollider);
		}
	}

	private void OnHitByExplosion()
	{
		explodeOnDeath = true;
		StartCoroutine(delayedExplosionCounter());
	}

	private void OnHeadChopped()
	{
		headChopped = true;
		StartCoroutine(delayedHeadChopCounter());
	}

	private IEnumerator delayedHeadChopCounter()
	{
		yield return new WaitForSeconds(0.2f);
		headChopped = false;
	}

	private IEnumerator delayedExplosionCounter()
	{
		yield return new WaitForSeconds(0.2f);
		explodeOnDeath = false;
	}

	private void OnObjectDestroyed(GameObject obj)
	{
		if (isDying)
		{
			return;
		}
		isDying = true;
		StatsManager.Instance.totalHuggablesKilled++;
		int totalHuggablesKilled = StatsManager.Instance.totalHuggablesKilled;
		int num2 = 9000;
		StatsManager.Instance.currentScore += scoreValue;
		HUDController.Instance.updateScore();
		switch (base.name)
		{
		case "Ghost_Huggable":
			StatsManager.Instance.roygbivString = "";
			StatsManager.Instance.currentGhostsKilled++;
			break;
		case "HuggaBull_Huggable":
			if (StatsManager.Instance.roygbivString == "R")
			{
				StatsManager.Instance.roygbivString = "O";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.currentHuggabullsKilled++;
			break;
		case "Yellow_Crusher":
			if (StatsManager.Instance.roygbivString == "O")
			{
				StatsManager.Instance.roygbivString = "Y";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.currentCrushersKilled++;
			break;
		case "Pink_Huggable":
		{
			StatsManager.Instance.roygbivString = "";
			StatsManager.Instance.pinksInRow++;
			int pinksInRow = StatsManager.Instance.pinksInRow;
			int num3 = 20;
			StatsManager.Instance.currentPinksKilled++;
			break;
		}
		case "Orange_Huggable":
			if (StatsManager.Instance.roygbivString == "R")
			{
				StatsManager.Instance.roygbivString = "O";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentOrangesKilled++;
			break;
		case "Green_Huggable":
			if (StatsManager.Instance.roygbivString == "Y")
			{
				StatsManager.Instance.roygbivString = "G";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentGreensKilled++;
			break;
		case "Projectile_Huggable":
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentProjectilesKilled++;
			break;
		case "Red_Huggable":
			if (StatsManager.Instance.roygbivString == "")
			{
				StatsManager.Instance.roygbivString = "R";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentRedsKilled++;
			break;
		case "Yellow_Huggable":
			if (StatsManager.Instance.roygbivString == "O")
			{
				StatsManager.Instance.roygbivString = "Y";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentYellowsKilled++;
			break;
		case "Blue_Huggable":
			if (!(StatsManager.Instance.roygbivString == "G"))
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentBluesKilled++;
			if (headChopped)
			{
				StatsManager.Instance.currentBluesKilledByKatana++;
				if (StatsManager.Instance.currentBluesKilledByKatana != 20)
				{
				}
			}
			break;
		case "Secret_Huggable":
			StatsManager.Instance.roygbivString = "";
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentSecretsKilled++;
			break;
		case "HuggaBull_HuggableZombie":
			if (StatsManager.Instance.roygbivString == "R")
			{
				StatsManager.Instance.roygbivString = "O";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.currentHuggabullsKilled++;
			break;
		case "Yellow_CrusherZombie":
			if (StatsManager.Instance.roygbivString == "O")
			{
				StatsManager.Instance.roygbivString = "Y";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.currentCrushersKilled++;
			break;
		case "Pink_HuggableZombie":
		{
			StatsManager.Instance.roygbivString = "";
			StatsManager.Instance.pinksInRow++;
			int pinksInRow2 = StatsManager.Instance.pinksInRow;
			int num4 = 20;
			StatsManager.Instance.currentPinksKilled++;
			break;
		}
		case "Orange_HuggableZombie":
			if (StatsManager.Instance.roygbivString == "R")
			{
				StatsManager.Instance.roygbivString = "O";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentOrangesKilled++;
			break;
		case "Green_HuggableZombie":
			if (StatsManager.Instance.roygbivString == "Y")
			{
				StatsManager.Instance.roygbivString = "G";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentGreensKilled++;
			break;
		case "Projectile_HuggableZombie":
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentProjectilesKilled++;
			break;
		case "Red_HuggableZombie":
			if (StatsManager.Instance.roygbivString == "")
			{
				StatsManager.Instance.roygbivString = "R";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentRedsKilled++;
			break;
		case "Yellow_HuggableZombie":
			if (StatsManager.Instance.roygbivString == "O")
			{
				StatsManager.Instance.roygbivString = "Y";
			}
			else
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentYellowsKilled++;
			break;
		case "Blue_HuggableZombie":
			if (!(StatsManager.Instance.roygbivString == "G"))
			{
				StatsManager.Instance.roygbivString = "";
			}
			StatsManager.Instance.pinksInRow = 0;
			StatsManager.Instance.currentBluesKilled++;
			if (headChopped)
			{
				StatsManager.Instance.currentBluesKilledByKatana++;
				if (StatsManager.Instance.currentBluesKilledByKatana != 20)
				{
				}
			}
			break;
		case "Pink_Huggable_noPickups":
		{
			StatsManager.Instance.roygbivString = "";
			StatsManager.Instance.pinksInRow++;
			int pinksInRow3 = StatsManager.Instance.pinksInRow;
			int num5 = 20;
			StatsManager.Instance.currentPinksKilled++;
			break;
		}
		}
		if (currentPath != null)
		{
			currentPath.error = true;
		}
		currentBaddies.Remove(myCollider);
		hasRemovedSelf = true;
		myRigidbody.velocity = Vector3.zero;
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		currentTurnSpeed = 0f;
		if (explodeOnDeath)
		{
			spawnPickups();
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				spawnTreasure();
			}
			if (squishSound != null)
			{
				SoundManager.Instance.playSound(squishSound);
			}
			if (explodeSplatterEffect != null && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
			{
				GameObject obj2 = Object.Instantiate(explodeSplatterEffect);
				Vector3 position = myTransform.position;
				position.y = explodeHeight;
				obj2.transform.position = position;
			}
			OnFinalDeath();
			Object.Destroy(base.gameObject);
		}
		else
		{
			Object.Destroy(GetComponent(typeof(DamageReceiver)));
			Object.Destroy(GetComponent<Collider>());
			string text = "";
			if (headChopped)
			{
				text = "HugDeathB";
			}
			else
			{
				int num = Random.Range(0, deathAnimations.Length);
				text = deathAnimations[num];
			}
			StopAllCoroutines();
			StartCoroutine(playDeath(text));
		}
	}

	protected virtual IEnumerator playDeath(string deathAnimName)
	{
		StartCoroutine(lodChecker());
		if (myAnimation[deathAnimName] == null)
		{
			int num = Random.Range(0, deathAnimations.Length);
			deathAnimName = deathAnimations[num];
		}
		if (currentMeleeAttack != null)
		{
			Object.Destroy(currentMeleeAttack);
			currentMeleeAttack = null;
		}
		myRigidbody.detectCollisions = false;
		if (deathAnimName == "HugDeathB" && headshotSound != null)
		{
			if (headshotRainbow != null)
			{
				Object.Instantiate(headshotRainbow).transform.position = myTransform.position;
			}
			SoundManager.Instance.playSound(headshotSound);
		}
		else if (deathSound != null)
		{
			SoundManager.Instance.playSound(deathSound);
		}
		myAnimation[deathAnimName].wrapMode = WrapMode.Once;
		myAnimation.CrossFade(deathAnimName);
		yield return new WaitForSeconds(myAnimation[deathAnimName].length);
		Transform transform = base.transform.Find("shadow");
		if (transform != null)
		{
			Object.Destroy(transform.gameObject);
		}
		OnFinalDeath();
		yield return new WaitForSeconds(3f);
		spawnPickups();
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			spawnTreasure();
		}
		myRigidbody.useGravity = false;
		myRigidbody.velocity = new Vector3(0f, -1f, 0f);
		if (deathEffect != null && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
		{
			GameObject obj = Object.Instantiate(deathEffect);
			Vector3 position = myTransform.position;
			position.y = 0f;
			obj.transform.position = position;
		}
		yield return new WaitForSeconds(2f);
		Object.Destroy(base.gameObject);
	}

	private void spawnTreasure()
	{
		float num = 0f;
		float value = Random.value;
		for (int i = 0; i < treasureDropProbabilities.Length; i++)
		{
			num += treasureDropProbabilities[i];
			if (value <= num)
			{
				for (int j = 0; j < treasureToDrop; j++)
				{
					int num2 = 0;
					num2 = ((!spawnAllSameType) ? ((i + j) % treasureDrops.Length) : i);
					GameObject obj = Object.Instantiate(treasureDrops[num2]);
					Vector2 vector = Random.insideUnitCircle * 2.5f;
					obj.transform.position = new Vector3(myTransform.position.x + vector.x, 1f, myTransform.position.z + vector.y);
					obj.transform.rotation = Quaternion.identity;
					obj.transform.parent = myTransform.parent;
				}
				break;
			}
		}
	}

	private void spawnPickups()
	{
		float num = 0f;
		float value = Random.value;
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			numberToDrop = 1;
		}
		for (int i = 0; i < dropProbabilities.Length; i++)
		{
			num += dropProbabilities[i];
			if (!(value <= num))
			{
				continue;
			}
			for (int j = 0; j < numberToDrop; j++)
			{
				int num2 = 0;
				num2 = ((!spawnAllSameType) ? ((i + j) % deathDrops.Length) : i);
				if (!GameManager.Instance.hasAcquiredSpecial && deathDrops[num2].name == "specialPickup")
				{
					break;
				}
				GameObject obj = Object.Instantiate(deathDrops[num2]);
				Vector2 vector = Random.insideUnitCircle * 2.5f;
				obj.transform.position = new Vector3(myTransform.position.x + vector.x, 1f, myTransform.position.z + vector.y);
				obj.transform.rotation = Quaternion.identity;
				obj.transform.parent = myTransform.parent;
			}
			break;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			currentTarget.SendMessage("OnHit", collisionDamage * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.tag == "Player")
		{
			currentTarget.SendMessage("OnHit", collisionDamage * GameManager.Instance.getDamageMultiplier() * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
		}
	}

	private IEnumerator deathAfterShot()
	{
		if (currentPath != null)
		{
			currentPath.error = true;
		}
		isDying = true;
		currentBaddies.Remove(myCollider);
		hasRemovedSelf = true;
		myRigidbody.velocity = Vector3.zero;
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		currentTurnSpeed = 0f;
		Object.Destroy(GetComponent(typeof(DamageReceiver)));
		Object.Destroy(GetComponent<Collider>());
		StartCoroutine(lodChecker());
		yield return new WaitForSeconds(myAnimation["Huggable_Projectile"].length - 1f);
		Object.Destroy(base.transform.Find("shadow").gameObject);
		OnFinalDeath();
		yield return new WaitForSeconds(3f);
		myRigidbody.useGravity = false;
		myRigidbody.detectCollisions = false;
		myRigidbody.velocity = new Vector3(0f, -1f, 0f);
		yield return new WaitForSeconds(2f);
		Object.Destroy(base.gameObject);
	}

	protected virtual void Update()
	{
		if (currentTarget != null && !isDying && Vector3.Distance(myCollider.bounds.center, currentTarget.position) < 2.5f)
		{
			currentMoveSpeed = 0f;
			currentWaypointIndex = -1;
			currentLookTarget = currentTarget.position;
			myRigidbody.velocity = Vector3.zero;
		}
		if (_delayCheckNotStart > 0f)
		{
			_delayCheckNotStart -= Time.deltaTime;
			if (_delayCheckNotStart <= 0f && !hasEnteredPlay)
			{
				hasEnteredPlay = true;
				dmgReceiver.isInvincible = false;
				currentMoveSpeed = moveSpeed;
				StartCoroutine(mainBehaviour());
			}
		}
	}

	protected virtual IEnumerator mainBehaviour()
	{
		yield return new WaitForSeconds(0.05f);
		while (currentTarget != null && !isDying)
		{
			if (Time.time > lastIdleTime + idleDelay)
			{
				moveLog += "-Idle";
				yield return StartCoroutine(idle(Random.Range(0.2f, 1f)));
				lastIdleTime = Time.time;
				idleDelay = Random.Range(minIdleTime, maxIdleTime);
				continue;
			}
			float num = Vector3.Distance(myCollider.bounds.center, currentTarget.position);
			if (num < attackDistance)
			{
				if (explodeOnAttack)
				{
					yield return StartCoroutine(explodeAttack());
				}
				else
				{
					yield return StartCoroutine(attack());
				}
			}
			else if (isRanged)
			{
				if (Random.value < 0.1f)
				{
					Vector3 direction = currentTarget.position - myCollider.bounds.center;
					direction.Normalize();
					RaycastHit hitInfo;
					if (Physics.Raycast(myCollider.bounds.center, direction, out hitInfo, num * 1.5f, shootMask))
					{
						if (hitInfo.transform == currentTarget)
						{
							myAnimation["Huggable_Projectile"].wrapMode = WrapMode.Once;
							myAnimation.CrossFade("Huggable_Projectile");
							yield return new WaitForSeconds(1f);
							GameObject gameObject = Object.Instantiate(rangedAttack);
							Transform transform = gameObject.transform;
							transform.position = attackSpawnpoint.position;
							transform.LookAt(currentTarget);
							Collider component = gameObject.GetComponent<Collider>();
							for (int i = 0; i < currentBaddies.Count; i++)
							{
								Physics.IgnoreCollision(currentBaddies[i] as Collider, component);
							}
							for (int j = 0; j < PickupController.currentPickups.Count; j++)
							{
								Physics.IgnoreCollision(PickupController.currentPickups[j] as Collider, component);
							}
							Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
							component2.velocity = transform.forward * rangedFiringForce;
							component2.angularVelocity = Random.insideUnitSphere * 5f;
							StopAllCoroutines();
							StartCoroutine(deathAfterShot());
						}
						else
						{
							yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
						}
					}
					else
					{
						yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
					}
				}
				else
				{
					yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
				}
			}
			else
			{
				yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
			}
		}
	}

	private IEnumerator explodeAttack()
	{
		if (currentPath != null)
		{
			currentPath.error = true;
		}
		isDying = true;
		currentBaddies.Remove(myCollider);
		hasRemovedSelf = true;
		Object.Destroy(GetComponent(typeof(DamageReceiver)));
		currentMoveSpeed = 0f;
		currentTurnSpeed = 0f;
		string animation = "";
		switch (Random.Range(0, 2))
		{
		case 0:
			animation = "HugExplodeA";
			break;
		case 1:
			animation = "HugExplodeB";
			break;
		}
		myAnimation[animation].wrapMode = WrapMode.Once;
		myAnimation[animation].speed = attackAnimationSpeedMultiplier;
		myAnimation.CrossFade(animation);
		yield return new WaitForSeconds(myAnimation[animation].length / attackAnimationSpeedMultiplier);
		GameObject obj = Object.Instantiate(meleeAttack);
		obj.transform.position = attackSpawnpoint.position;
		obj.transform.rotation = attackSpawnpoint.rotation;
		(obj.GetComponent(typeof(OneTimeDamageDealer)) as OneTimeDamageDealer).damage *= GameManager.Instance.getDamageMultiplier();
		if (explodeSound != null)
		{
			SoundManager.Instance.playSound(explodeSound);
		}
		OnFinalDeath();
		Object.Destroy(base.gameObject);
		StopAllCoroutines();
	}

	protected virtual IEnumerator attack()
	{
		if (currentPath != null)
		{
			currentPath.error = true;
		}
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		currentLookTarget = currentTarget.position;
		myRigidbody.velocity = Vector3.zero;
		int num = Random.Range(0, attackAnimations.Length);
		string attackAnimation = attackAnimations[num];
		if (attackSounds.Length != 0)
		{
			SoundManager.Instance.playSound(attackSounds[Random.Range(0, attackSounds.Length)]);
		}
		myAnimation[attackAnimation].wrapMode = WrapMode.Once;
		myAnimation[attackAnimation].speed = attackAnimationSpeedMultiplier;
		myAnimation.CrossFade(attackAnimation);
		yield return new WaitForSeconds(myAnimation[attackAnimation].length / (2f * attackAnimationSpeedMultiplier));
		if (meleeAttack != null)
		{
			if (currentMeleeAttack == null)
			{
				GameObject gameObject = Object.Instantiate(meleeAttack);
				gameObject.transform.position = attackSpawnpoint.position;
				gameObject.transform.rotation = attackSpawnpoint.rotation;
				currentMeleeAttack = gameObject;
			}
			else
			{
				currentMeleeAttack.transform.position = attackSpawnpoint.position;
				currentMeleeAttack.transform.rotation = attackSpawnpoint.rotation;
			}
		}
		yield return new WaitForSeconds(myAnimation[attackAnimation].length / (2f * attackAnimationSpeedMultiplier));
	}

	protected IEnumerator chase(float chaseTime)
	{
		currentMoveSpeed = moveSpeed;
		myAnimation[walkAnimation].wrapMode = WrapMode.Loop;
		myAnimation[walkAnimation].speed = moveSpeed / (2.5f * moveAnimationSpeedDivider);
		myAnimation.CrossFade(walkAnimation);
		float maxDistance = Vector3.Distance(currentTarget.position, myCollider.bounds.center);
		Vector3 vector = currentTarget.position - myCollider.bounds.center;
		vector.Normalize();
		Vector3 vector2 = new Vector3(0f - vector.z, vector.y, vector.x) * myCollider.bounds.extents.x * 1.5f;
		Vector3 vector3 = myCollider.bounds.center + vector2;
		Vector3 vector4 = myCollider.bounds.center - vector2;
		Vector3 vector5 = myCollider.bounds.center - vector;
		(currentTarget.position - vector3).Normalize();
		(currentTarget.position - vector4).Normalize();
		Vector3 center = myCollider.bounds.center;
		Vector3 position = currentTarget.position;
		position.y = 1f;
		RaycastHit hitInfo;
		if (!Physics.Raycast(vector3, vector, out hitInfo, maxDistance, chaseMask))
		{
			if (!Physics.Raycast(vector4, vector, out hitInfo, maxDistance, chaseMask))
			{
				if (!Physics.CapsuleCast(center, position, 0.1f, vector, out hitInfo, maxDistance, chaseMask))
				{
					waypoints.Clear();
					waypoints.Add(new Vector3(currentTarget.position.x, 0f, currentTarget.position.z));
					currentWaypointIndex = 0;
					moveLog += "-Di thang";
					yield return new WaitForSeconds(chaseTime);
				}
				else
				{
					moveLog += "-calculatePath1";
					yield return StartCoroutine(calculatePath(currentTarget));
					yield return new WaitForSeconds(chaseTime * pathDelay);
				}
			}
			else
			{
				moveLog += "-calculatePath2";
				yield return StartCoroutine(calculatePath(currentTarget));
				yield return new WaitForSeconds(chaseTime * pathDelay);
			}
		}
		else
		{
			moveLog += "calculatePath3";
			yield return StartCoroutine(calculatePath(currentTarget));
			yield return new WaitForSeconds(chaseTime * pathDelay);
		}
	}

	private IEnumerator idle(float idleTime)
	{
		currentMoveSpeed = 0f;
		myAnimation[idleAnimation].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade(idleAnimation);
		yield return new WaitForSeconds(idleTime);
	}

	private IEnumerator calculatePath(Transform target)
	{
		StartPath(new Vector3(myTransform.position.x, 0f, myTransform.position.z), new Vector3(target.position.x, 0f, target.position.z));
		while (!currentPath.error && !currentPath.foundEnd)
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
			Debug.Log("no path!");
			myRigidbody.velocity = Vector3.zero;
			currentWaypointIndex = -1;
		}
	}

	private void StartPath(Vector3 start, Vector3 end)
	{
		StartCoroutine(AstarPath.StartPath(currentPath = new AstarPath.Path(start, end, maxAngle, angleCost, false)));
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

	private bool moveTowards(Vector3 targetPos, float timeElapsed)
	{
		Vector3 vector = targetPos - myTransform.position;
		vector.y = 0f;
		if (vector.magnitude < currentMoveSpeed * timeElapsed * 1.5f)
		{
			myTransform.position = targetPos;
			myRigidbody.velocity = Vector3.zero;
			currentLookTarget = currentTarget.position;
			if (!hasEnteredPlay)
			{
				hasEnteredPlay = true;
				dmgReceiver.isInvincible = false;
				currentMoveSpeed = moveSpeed;
				StartCoroutine(mainBehaviour());
			}
			return true;
		}
		vector.Normalize();
		myRigidbody.velocity = vector * currentMoveSpeed;
		return false;
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

	private void switchLOD()
	{
		if (!(LODmesh == null) && !(currentTarget == null))
		{
			float num = Vector3.Distance(currentTarget.position, myTransform.position);
			if (num > LODdistance && !isLOD)
			{
				isLOD = true;
				myRenderer.sharedMesh = LODmesh;
			}
			else if (num < LODdistance && isLOD)
			{
				isLOD = false;
				myRenderer.sharedMesh = highQualityMesh;
			}
		}
	}

	private IEnumerator lodChecker()
	{
		while (currentTarget != null)
		{
			switchLOD();
			yield return new WaitForSeconds(0.15f);
		}
	}

	private IEnumerator mover()
	{
		while (currentTarget != null)
		{
			myTransform.position = new Vector3(myTransform.position.x, 0f, myTransform.position.z);
			if (currentWaypointIndex >= 0 && currentWaypointIndex < waypoints.Count)
			{
				rotateTowards((Vector3)waypoints[currentWaypointIndex], 0.15f);
				if (moveTowards((Vector3)waypoints[currentWaypointIndex], 0.15f))
				{
					currentWaypointIndex++;
				}
			}
			else
			{
				rotateTowards(currentLookTarget, 0.15f);
			}
			yield return new WaitForSeconds(0.15f);
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
