using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBossController : MonoBehaviour
{
	private Animation myAnimation;

	private Transform myTransform;

	private Rigidbody myRigidbody;

	public GameObject rightHookCollider;

	public GameObject leftHookCollider;

	private bool onlyOneArmAttached;

	public Renderer shieldRenderer;

	public GameObject mainBody;

	public GameObject leftArm;

	public GameObject rightArm;

	public GameObject massiveDamage;

	public GameObject massiveDamageBackBlocker;

	private DamageReceiver mainBodyDmgReceiver;

	private DamageReceiver leftArmDmgReceiver;

	private DamageReceiver rightArmDmgReceiver;

	private DamageReceiver massiveDamageDmgReceiver;

	public Renderer mainBodyRenderer;

	public Renderer leftArmRenderer;

	public Renderer rightArmRenderer;

	public float currentHealth;

	public float maxHealth = 20f;

	public float maxRealHealth = 20f;

	public float minIdleTime = 2f;

	public float maxIdleTime = 8f;

	public float screenClearDistance = 100f;

	public float asteroidXRange = 5f;

	public float asteroidYRange = 5f;

	public float asteroidZRange = 5f;

	private bool isWhite;

	private Material originalMaterial;

	public Material materialWhenHit;

	public GameObject screenClearEffect;

	private Transform playerTransform;

	private bool inDamageSpin;

	public GameObject[] asteroids;

	public Transform asteroidSpawnpoint;

	public Transform throwingArm;

	public GameObject missilePrefab;

	public Transform[] missileSpawnpoints;

	public Collider playerBlockCollider;

	public List<Collider> listColliderIgnore = new List<Collider>();

	private GameObject asteroidInConstruction;

	private bool launchingMissiles;

	private bool shieldsFailed;

	private bool isDead;

	public AudioClip[] impacts;

	public AudioClip pushback;

	public AudioClip shieldDown;

	public AudioClip armDestroy;

	public AudioClip[] asteroidMakeSounds;

	public AudioClip bossMusic;

	public AudioClip tauntSound;

	private void Awake()
	{
		myTransform = base.transform;
		myRigidbody = GetComponent<Rigidbody>();
		myAnimation = GetComponent<Animation>();
		onlyOneArmAttached = false;
		mainBodyDmgReceiver = mainBody.GetComponent(typeof(DamageReceiver)) as DamageReceiver;
		leftArmDmgReceiver = leftArm.GetComponent(typeof(DamageReceiver)) as DamageReceiver;
		rightArmDmgReceiver = rightArm.GetComponent(typeof(DamageReceiver)) as DamageReceiver;
		massiveDamageDmgReceiver = massiveDamage.GetComponent(typeof(DamageReceiver)) as DamageReceiver;
		originalMaterial = mainBodyRenderer.sharedMaterial;
		minIdleTime -= (float)GameManager.Instance.currentDifficulty;
		maxIdleTime -= (float)GameManager.Instance.currentDifficulty;
	}

	private void Start()
	{
		playerTransform = GameObject.FindWithTag("Player").transform.root;
		maxHealth *= GameManager.Instance.bossHPMultipliers[(int)GameManager.Instance.currentDifficulty];
		massiveDamageDmgReceiver.hitpoints = maxRealHealth * GameManager.Instance.bossHPMultipliers[(int)GameManager.Instance.currentDifficulty];
		currentHealth = maxHealth;
		myAnimation["idleBothArmsAttached"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("idleBothArmsAttached");
		Color color = shieldRenderer.sharedMaterial.color;
		color.a = 0.5f;
		shieldRenderer.sharedMaterial.color = color;
		mainBodyDmgReceiver.listeners.Add(base.gameObject);
		leftArmDmgReceiver.listeners.Add(base.gameObject);
		rightArmDmgReceiver.listeners.Add(base.gameObject);
		mainBody.active = false;
		leftArm.active = false;
		rightArm.active = false;
		massiveDamage.active = false;
		massiveDamageBackBlocker.active = false;
		SoundManager.Instance.playMusic(bossMusic, true);
	}

	public void OnDamageTaken(float dmg)
	{
		currentHealth -= dmg;
		if (currentHealth < 0f)
		{
			currentHealth = maxHealth;
			string text = "";
			text = ((!(Random.value < 0.5f)) ? "DamageSpinB" : "DamageSpinA");
			StopAllCoroutines();
			mainBodyRenderer.sharedMaterial = originalMaterial;
			leftArmRenderer.sharedMaterial = originalMaterial;
			rightArmRenderer.sharedMaterial = originalMaterial;
			isWhite = false;
			if (asteroidInConstruction != null)
			{
				Object.Destroy(asteroidInConstruction);
				asteroidInConstruction = null;
			}
			launchingMissiles = false;
			StartCoroutine(damageSpin(text));
		}
		else if (!isWhite)
		{
			StartCoroutine(turnWhite());
		}
	}

	private IEnumerator distanceToPlayerChecker()
	{
		while (playerTransform != null)
		{
			if (Vector3.Distance(playerTransform.position, myTransform.position) < screenClearDistance && !inDamageSpin && asteroidInConstruction == null && !launchingMissiles)
			{
				StopAllCoroutines();
				mainBodyRenderer.sharedMaterial = originalMaterial;
				leftArmRenderer.sharedMaterial = originalMaterial;
				rightArmRenderer.sharedMaterial = originalMaterial;
				isWhite = false;
				StartCoroutine(screenClearAttack());
				StartCoroutine(screenClearPush());
				break;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	private IEnumerator screenClearAttack()
	{
		yield return new WaitForSeconds(0.1f);
		if (pushback != null)
		{
			SoundManager.Instance.playSound(pushback);
		}
		massiveDamageDmgReceiver.damageMultiplier = 1f;
		mainBody.active = false;
		leftArm.active = false;
		rightArm.active = false;
		massiveDamage.active = false;
		massiveDamageBackBlocker.active = false;
		myAnimation["ScreenClearAttack"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("ScreenClearAttack");
		Object.Instantiate(screenClearEffect).transform.position = myTransform.position;
		yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(myAnimation["ScreenClearAttack"].length);
		StartCoroutine("mainBehaviour");
	}

	private IEnumerator screenClearPush()
	{
		yield return new WaitForSeconds(0.5f);
		while (playerTransform != null)
		{
			if (Vector3.Distance(playerTransform.position, myTransform.position) > 200f)
			{
				StartCoroutine(distanceToPlayerChecker());
				break;
			}
			Vector3 position = playerTransform.position;
			position.z += 2f;
			playerTransform.position = position;
			yield return new WaitForSeconds(0.02f);
		}
	}

	private IEnumerator turnWhite()
	{
		isWhite = true;
		if (impacts.Length != 0)
		{
			SoundManager.Instance.playSound(impacts[Random.Range(0, impacts.Length)]);
		}
		mainBodyRenderer.sharedMaterial = materialWhenHit;
		leftArmRenderer.sharedMaterial = materialWhenHit;
		rightArmRenderer.sharedMaterial = materialWhenHit;
		yield return new WaitForSeconds(0.2f);
		mainBodyRenderer.sharedMaterial = originalMaterial;
		leftArmRenderer.sharedMaterial = originalMaterial;
		rightArmRenderer.sharedMaterial = originalMaterial;
		isWhite = false;
	}

	public void OnObjectDestroyed(GameObject obj)
	{
		if (isDead)
		{
			return;
		}
		if (obj == massiveDamage || obj == mainBody)
		{
			isDead = true;
			StartCoroutine(death());
		}
		if (!(obj == leftHookCollider) && !(obj == rightHookCollider))
		{
			return;
		}
		if (obj == leftHookCollider)
		{
			if (!onlyOneArmAttached)
			{
				myAnimation["LeftArmDetachRightAttach"].wrapMode = WrapMode.Once;
				myAnimation.Play("LeftArmDetachRightAttach");
				myAnimation["LeftArmDetachIdleRightAttach"].wrapMode = WrapMode.Loop;
				myAnimation.CrossFadeQueued("LeftArmDetachIdleRightAttach", 0.1f, QueueMode.CompleteOthers);
			}
			else
			{
				myAnimation["RightArmDetachLeftDetach"].wrapMode = WrapMode.Once;
				myAnimation.CrossFade("RightArmDetachLeftDetach");
			}
		}
		else if (obj == rightHookCollider)
		{
			if (!onlyOneArmAttached)
			{
				myAnimation["RightArmDetachLeftAttach"].wrapMode = WrapMode.Once;
				myAnimation.Play("RightArmDetachLeftAttach");
				myAnimation["RightArmDetachIdleLeftAttach"].wrapMode = WrapMode.Loop;
				myAnimation.CrossFadeQueued("RightArmDetachIdleLeftAttach", 0.1f, QueueMode.CompleteOthers);
			}
			else
			{
				myAnimation["LeftArmDetachRightDetach"].wrapMode = WrapMode.Once;
				myAnimation.CrossFade("LeftArmDetachRightDetach");
			}
		}
		if (onlyOneArmAttached)
		{
			myAnimation["idleA"].wrapMode = WrapMode.Loop;
			myAnimation.CrossFadeQueued("idleA", 0.1f, QueueMode.CompleteOthers);
			if (!shieldsFailed)
			{
				StartCoroutine(shieldFail());
				shieldsFailed = true;
			}
		}
		else
		{
			if (armDestroy != null)
			{
				SoundManager.Instance.playSound(armDestroy);
			}
			onlyOneArmAttached = true;
			StartCoroutine(shieldReduce());
		}
	}

	private IEnumerator death()
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
			float num3 = 1380f;
			int wasHit = StatsManager.Instance.wasHit;
			float num4 = (float)(StatsManager.Instance.currentPinksKilled + StatsManager.Instance.currentGreensKilled + StatsManager.Instance.currentOrangesKilled + StatsManager.Instance.currentProjectilesKilled + StatsManager.Instance.currentYellowsKilled + StatsManager.Instance.currentRedsKilled + StatsManager.Instance.currentBluesKilled + StatsManager.Instance.currentTurretsKilled + StatsManager.Instance.currentSecretsKilled) / (float)StatsManager.Instance.currentShotsFired;
			float num5 = 0.25f;
			float num6 = 0.5f;
			float num7 = 1f;
			float num8 = 2f;
		}
		((Object.Instantiate(Resources.Load("FaderSystem")) as GameObject).GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1f;
		if (GameManager.Instance.currentBossTrial == BossMode.NONE)
		{
			yield return new WaitForSeconds(0.2f);
			PlayerPrefs.SetString("SpacebossUnlocked", "unlocked");
			if (GameManager.Instance.oliverUnlockedStage == 2)
			{
				GameManager.Instance.oliverUnlockedStage++;
				PlayerPrefs.SetInt("oliverUnlockedStage", GameManager.Instance.oliverUnlockedStage);
			}
			GameManager.Instance.PlayMovie("oco_spaceboss_win");
		}
		yield return new WaitForSeconds(0.1f);
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (!(gameObject.name == "GameManager") && !(gameObject.transform.root.name == "SoundManager") && !(gameObject.name == "Controlls") && !(gameObject.name == "Rewired Input Manager") && !(gameObject.name == "UICanvas") && !(gameObject.name == "EventSystem"))
			{
				Object.Destroy(gameObject);
			}
		}
		Time.timeScale = 1f;
		Object.Instantiate((Object.FindObjectOfType(typeof(EndMenuSpawner)) as EndMenuSpawner).endMenu);
	}

	private IEnumerator mainBehaviour()
	{
		StartCoroutine(distanceToPlayerChecker());
		mainBody.active = true;
		leftArm.active = true;
		rightArm.active = true;
		massiveDamage.active = false;
		massiveDamageBackBlocker.active = false;
		isWhite = false;
		while (true)
		{
			if (Random.value < 0.5f)
			{
				yield return StartCoroutine(asteroidAttack());
			}
			else
			{
				yield return StartCoroutine(missileAttack());
			}
			float value = Random.value;
			float idleTime = minIdleTime + (maxIdleTime - minIdleTime) * Random.value;
			string idleType;
			if (value < 0.33f)
			{
				idleType = "idleA";
			}
			else if (value < 0.5f)
			{
				massiveDamageDmgReceiver.damageMultiplier = 0.05f;
				mainBody.active = false;
				leftArm.active = false;
				rightArm.active = false;
				massiveDamage.active = true;
				massiveDamageBackBlocker.active = true;
				idleType = "idleB";
				StartCoroutine(playDelayedTaunt());
			}
			else
			{
				idleType = "idleC";
			}
			yield return StartCoroutine(idle(idleType, idleTime));
		}
	}

	private IEnumerator playDelayedTaunt()
	{
		yield return new WaitForSeconds(1.5f * Time.timeScale);
		SoundManager.Instance.playSound(tauntSound);
	}

	private IEnumerator damageSpin(string stunType)
	{
		inDamageSpin = true;
		massiveDamageDmgReceiver.damageMultiplier = 1f;
		mainBody.active = false;
		leftArm.active = false;
		rightArm.active = false;
		massiveDamage.active = true;
		massiveDamageBackBlocker.active = true;
		float num;
		if (stunType == "DamageSpinB")
		{
			num = 0.5f;
			myAnimation["DamageSpinB"].speed = 0.5f;
		}
		else
		{
			num = 1f;
			myAnimation["DamageSpinA"].speed = 1f;
		}
		myAnimation[stunType].wrapMode = WrapMode.Once;
		myAnimation.CrossFade(stunType);
		yield return new WaitForSeconds(myAnimation[stunType].length / num);
		inDamageSpin = false;
		StartCoroutine("mainBehaviour");
	}

	private IEnumerator asteroidAttack()
	{
		if (!(asteroidInConstruction == null))
		{
			yield break;
		}
		myAnimation["AsteroidThrow"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("AsteroidThrow");
		StartCoroutine("asteroidMakingSounds");
		yield return new WaitForSeconds(1f);
		GameObject newAsteroid = Object.Instantiate(asteroids[Random.Range(0, asteroids.Length)]);
		newAsteroid.transform.position = asteroidSpawnpoint.position;
		newAsteroid.transform.localScale = new Vector3(2f, 2f, 2f);
		asteroidInConstruction = newAsteroid;
		Collider component = newAsteroid.GetComponent<Collider>();
		for (int i = 0; i < listColliderIgnore.Count; i++)
		{
			Physics.IgnoreCollision(component, listColliderIgnore[i]);
		}
		yield return new WaitForSeconds(2.5f);
		StopCoroutine("asteroidMakingSounds");
		if (newAsteroid != null)
		{
			newAsteroid.transform.parent = throwingArm;
			yield return new WaitForSeconds(myAnimation["AsteroidThrow"].length - 4.6f);
			if (newAsteroid != null)
			{
				asteroidInConstruction = null;
				newAsteroid.transform.parent = null;
				yield return null;
				Rigidbody component2 = newAsteroid.GetComponent<Rigidbody>();
				Vector3 force = new Vector3(0f - asteroidXRange + Random.value * asteroidXRange * 2f, 2f + (0f - asteroidYRange + Random.value * asteroidYRange * 2f), 10f + (0f - asteroidZRange) + Random.value * asteroidZRange * 2f + 2f * (float)GameManager.Instance.currentDifficulty);
				force *= 1.6f;
				component2.AddForce(force, ForceMode.VelocityChange);
				component2.angularVelocity = Random.insideUnitSphere * 1.5f;
			}
		}
	}

	private IEnumerator asteroidMakingSounds()
	{
		while (true)
		{
			int i = 0;
			while (i < asteroidMakeSounds.Length)
			{
				SoundManager.Instance.playSound(asteroidMakeSounds[i]);
				yield return new WaitForSeconds(asteroidMakeSounds[i].length);
				int num = i + 1;
				i = num;
			}
		}
	}

	private IEnumerator missileAttack()
	{
		launchingMissiles = true;
		myAnimation["MissileLaunch"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("MissileLaunch");
		int i = 0;
		while (i < missileSpawnpoints.Length)
		{
			GameObject newMissile = Object.Instantiate(missilePrefab);
			newMissile.transform.position = missileSpawnpoints[i].position;
			Collider component = newMissile.GetComponent<Collider>();
			for (int j = 0; j < listColliderIgnore.Count; j++)
			{
				Physics.IgnoreCollision(component, listColliderIgnore[j]);
			}
			yield return new WaitForSeconds(0.1f);
			if (newMissile != null)
			{
				Rigidbody component2 = newMissile.GetComponent<Rigidbody>();
				if (component2 != null)
				{
					component2.AddForce(new Vector3(0f, 0f, 15f + 3f * (float)GameManager.Instance.currentDifficulty), ForceMode.VelocityChange);
					component2.angularVelocity = new Vector3(0f, 0f, 1f);
				}
			}
			yield return new WaitForSeconds(0.5f);
			int num = i + 1;
			i = num;
		}
		yield return new WaitForSeconds(myAnimation["MissileLaunch"].length - 2f);
		launchingMissiles = false;
	}

	private IEnumerator idle(string idleType, float idleTime)
	{
		myAnimation[idleType].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade(idleType);
		yield return new WaitForSeconds(idleTime);
		if (idleType == "idleB")
		{
			massiveDamageDmgReceiver.damageMultiplier = 1f;
			mainBody.active = true;
			leftArm.active = true;
			rightArm.active = true;
			massiveDamage.active = false;
			massiveDamageBackBlocker.active = false;
		}
	}

	private IEnumerator shieldReduce()
	{
		if (leftHookCollider != null)
		{
			leftHookCollider.active = false;
		}
		if (rightHookCollider != null)
		{
			rightHookCollider.active = false;
		}
		Color c = shieldRenderer.sharedMaterial.color;
		while (c.a > 0.25f)
		{
			c.a -= 0.01f;
			shieldRenderer.sharedMaterial.color = c;
			yield return new WaitForSeconds(0.04f);
		}
		if (leftHookCollider != null)
		{
			leftHookCollider.active = true;
		}
		if (rightHookCollider != null)
		{
			rightHookCollider.active = true;
		}
	}

	private IEnumerator shieldFail()
	{
		if (leftHookCollider != null)
		{
			leftHookCollider.active = false;
		}
		if (rightHookCollider != null)
		{
			rightHookCollider.active = false;
		}
		Color c = shieldRenderer.sharedMaterial.color;
		if (shieldDown != null)
		{
			SoundManager.Instance.playSound(shieldDown);
		}
		while (c.a > 0f)
		{
			c.a -= 0.01f;
			shieldRenderer.sharedMaterial.color = c;
			yield return new WaitForSeconds(0.04f);
		}
		Object.Destroy(shieldRenderer.gameObject);
		mainBody.active = true;
		leftArm.active = true;
		rightArm.active = true;
		massiveDamage.active = false;
		massiveDamageBackBlocker.active = false;
		StartCoroutine("mainBehaviour");
	}
}
