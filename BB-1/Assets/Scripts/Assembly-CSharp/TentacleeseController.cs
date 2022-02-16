using System.Collections;
using UnityEngine;

public class TentacleeseController : MonoBehaviour
{
	public GameObject stalk;

	public GameObject mainBody;

	public TentacleController leftBackTentacle;

	public TentacleController leftFrontTentacle;

	public TentacleController rightBackTentacle;

	public TentacleController rightFrontTentacle;

	private Animation stalkAnimation;

	private Animation bodyAnimation;

	private int numTentaclesLost;

	public bool isHanging = true;

	public Material whiteMaterial;

	private Material originalMaterial;

	public Renderer myRenderer;

	public float maxHP = 50f;

	private float currentHP;

	private int numTimesDamaged;

	public float stunTime = 10f;

	private bool isWhite;

	public ProjectileControllerPrefab spitter;

	private Transform currentTarget;

	private Transform myTransform;

	private bool isStunned;

	private bool isMissingUdder;

	public Material darkMaterial;

	public Renderer topStalkRenderer;

	private Component[] colliders;

	public GameObject secretCollider;

	public float secretHP = 50f;

	private bool isSpinning;

	public GameObject udder;

	public AudioClip bodyDamage;

	public AudioClip spitSound;

	public AudioClip bossMusic;

	private bool hasRecentlyMadeSound;

	private void Awake()
	{
		colliders = GetComponentsInChildren(typeof(Collider));
		Component[] array = colliders;
		foreach (Component component in array)
		{
			Component[] array2 = colliders;
			foreach (Component component2 in array2)
			{
				if (component != component2)
				{
					Physics.IgnoreCollision(component as Collider, component2 as Collider);
				}
			}
		}
		myTransform = base.transform;
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
		stalkAnimation = stalk.GetComponent<Animation>();
		bodyAnimation = mainBody.GetComponent<Animation>();
		bodyAnimation["MainBodyHangingIdle"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyHangingIdle");
	}

	private void Start()
	{
		secretCollider.active = false;
		currentTarget = getRandomTarget();
		SoundManager.Instance.playMusic(bossMusic, true);
		InitTarget();
		StartCoroutine(spitterLoop());
	}

	private void InitTarget()
	{
		CapsuleCollider componentInChildren = currentTarget.GetComponentInChildren<CapsuleCollider>();
		if (componentInChildren != null)
		{
			componentInChildren.isTrigger = false;
		}
	}

	private void OnUdderDetached()
	{
		isMissingUdder = true;
		BroadcastMessage("OnGoInactive");
		bodyAnimation.Stop();
		myRenderer.sharedMaterial = darkMaterial;
		GameManager.Instance.inUdderMode = true;
	}

	private IEnumerator spitterLoop()
	{
		while (currentTarget != null && !isMissingUdder)
		{
			yield return new WaitForSeconds(Random.value * 10f + 10f);
			int numSpits = 0;
			if (GameManager.Instance.currentDifficulty == GameDifficulty.EASY)
			{
				numSpits = 2;
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.MEDIUM)
			{
				numSpits = 3;
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.HARD)
			{
				numSpits = 5;
			}
			int i = 0;
			while (i < numSpits)
			{
				if (HuggableController.currentBaddies.Count < 20 && !isStunned && !isMissingUdder)
				{
					spitter.transform.LookAt(currentTarget);
					spitter.FireProjectile(myTransform, 1);
					SoundManager.Instance.playSound(spitSound);
					yield return new WaitForSeconds(1f);
				}
				int num = i + 1;
				i = num;
			}
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

	private IEnumerator temporarySoundDisable()
	{
		hasRecentlyMadeSound = true;
		yield return new WaitForSeconds(1f);
		hasRecentlyMadeSound = false;
	}

	public void OnBodyHit(float damage)
	{
		if (currentHP <= 0f || !isHanging || isStunned || isMissingUdder)
		{
			return;
		}
		if (!hasRecentlyMadeSound)
		{
			SoundManager.Instance.playSound(bodyDamage);
			StartCoroutine(temporarySoundDisable());
		}
		if (!isWhite)
		{
			StartCoroutine(turnWhite());
		}
		currentHP -= damage;
		if (currentHP <= 0f)
		{
			numTimesDamaged++;
			if (numTimesDamaged < 4)
			{
				StartCoroutine(OnBodyDamaged());
			}
			else
			{
				StartCoroutine(OnBodyStunned());
			}
		}
	}

	private IEnumerator OnBodyStunned()
	{
		BroadcastMessage("OnStunned");
		isStunned = true;
		bodyAnimation["MainBodyStunIdle"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyStunIdle");
		yield return new WaitForSeconds(stunTime);
		bodyAnimation["MainBodyHangingIdle"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyHangingIdle");
		BroadcastMessage("OnRecovered");
		isStunned = false;
		currentHP = maxHP;
		numTimesDamaged = 0;
	}

	private IEnumerator turnWhite()
	{
		isWhite = true;
		myRenderer.sharedMaterial = whiteMaterial;
		yield return new WaitForSeconds(0.2f);
		myRenderer.sharedMaterial = originalMaterial;
		isWhite = false;
	}

	public void OnTentacleDetached()
	{
		if (!isHanging)
		{
			return;
		}
		numTentaclesLost++;
		if (numTentaclesLost < 4)
		{
			OnBodyHit(25f);
			return;
		}
		StopAllCoroutines();
		myRenderer.sharedMaterial = originalMaterial;
		isWhite = false;
		if ((udder.GetComponent(typeof(UdderController)) as UdderController).isHanging)
		{
			StartCoroutine(OnBodyFall());
		}
	}

	private IEnumerator OnBodyFall()
	{
		topStalkRenderer.enabled = false;
		bodyAnimation["MainBodyLandingFromDetachStalk"].wrapMode = WrapMode.Once;
		bodyAnimation.CrossFade("MainBodyLandingFromDetachStalk");
		isHanging = false;
		yield return new WaitForSeconds(bodyAnimation["MainBodyLandingFromDetachStalk"].length + 0.01f);
		BroadcastMessage("OnBodyHasFallen");
		StartCoroutine(OnFacePlayer());
		StartCoroutine(spawner());
		StartCoroutine(attackWithTentacles());
		bodyAnimation["MainBodyDetachIdle"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyDetachIdle");
		secretCollider.active = true;
		Object.Destroy(udder);
	}

	private IEnumerator spawner()
	{
		while (currentTarget != null)
		{
			yield return new WaitForSeconds(Random.value * 10f + 5f);
			int numSpits = 0;
			if (GameManager.Instance.currentDifficulty == GameDifficulty.EASY)
			{
				numSpits = 2;
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.MEDIUM)
			{
				numSpits = 3;
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.HARD)
			{
				numSpits = 5;
			}
			int i = 0;
			while (i < numSpits)
			{
				if (HuggableController.currentBaddies.Count < 20)
				{
					Vector3 eulerAngles = spitter.transform.eulerAngles;
					eulerAngles.x = 90f;
					eulerAngles.z = 0f;
					eulerAngles.y = 0f;
					spitter.transform.eulerAngles = eulerAngles;
					spitter.FireProjectile(myTransform, 1);
					SoundManager.Instance.playSound(spitSound);
					yield return new WaitForSeconds(1f);
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	private void OnSecretColliderHit(float damage)
	{
		if (!(secretHP <= 0f))
		{
			if (!isWhite)
			{
				StartCoroutine(turnWhite());
			}
			if (!hasRecentlyMadeSound)
			{
				SoundManager.Instance.playSound(bodyDamage);
				StartCoroutine(temporarySoundDisable());
			}
			secretHP -= damage;
			if (secretHP <= 0f)
			{
				StopAllCoroutines();
				StartCoroutine(OnFinalDeath());
			}
			else if (!isSpinning)
			{
				StartCoroutine(OnRandomSpin());
			}
		}
	}

	private void OnGroundTentacleDetached(TentacleController t)
	{
		if ((leftFrontTentacle == null && t == rightFrontTentacle) || (rightFrontTentacle == null && t == leftFrontTentacle))
		{
			StartCoroutine(playTentacleDeath());
		}
	}

	private IEnumerator playTentacleDeath()
	{
		bodyAnimation["MainBodyDetachDeath"].wrapMode = WrapMode.Once;
		bodyAnimation.CrossFade("MainBodyDetachDeath");
		yield return new WaitForSeconds(bodyAnimation["MainBodyDetachDeath"].length);
		bodyAnimation["MainBodyDetachDeathIdle"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyDetachDeathIdle");
	}

	private IEnumerator OnRandomSpin()
	{
		float timeToSpin = 5f;
		float firingInterval = 0f;
		bodyAnimation["MainBodyDetachDamage"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyDetachDamage");
		while (timeToSpin > 0f)
		{
			Vector3 eulerAngles = myTransform.eulerAngles;
			eulerAngles.y += 10f;
			myTransform.eulerAngles = eulerAngles;
			firingInterval += 0.04f;
			if (firingInterval > 1f)
			{
				firingInterval = 0f;
				spitter.transform.eulerAngles = eulerAngles;
				spitter.FireProjectile(myTransform, 1);
				SoundManager.Instance.playSound(spitSound);
			}
			yield return new WaitForSeconds(0.04f);
			timeToSpin -= 0.04f;
		}
		bodyAnimation["MainBodyDetachDeathIdle"].wrapMode = WrapMode.Loop;
		bodyAnimation.CrossFade("MainBodyDetachDeathIdle");
		isSpinning = false;
	}

	private IEnumerator OnFinalDeath()
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
			GameMode currentGameMode = GameManager.Instance.currentGameMode;
		}
		yield return new WaitForSeconds(0.1f);
		if (GameManager.Instance.currentGameMode != 0)
		{
			float scale = myTransform.localScale.x;
			while (scale > 0f)
			{
				scale -= 2f;
				myTransform.localScale = new Vector3(scale, scale, scale);
				Vector3 eulerAngles = myTransform.eulerAngles;
				eulerAngles.y += 17f;
				myTransform.eulerAngles = eulerAngles;
				yield return new WaitForSeconds(0.04f);
			}
		}
		Camera[] allCameras = Camera.allCameras;
		foreach (Camera camera in allCameras)
		{
			if (camera.gameObject.name != "clearCamera")
			{
				camera.enabled = false;
			}
		}
		if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
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
			GameManager.Instance.PlayMovie("r_boss1_win");
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

	private IEnumerator OnFacePlayer()
	{
		while (currentTarget != null && (rightFrontTentacle != null || leftFrontTentacle != null))
		{
			myTransform.LookAt(currentTarget);
			Vector3 eulerAngles = myTransform.eulerAngles;
			eulerAngles.y += 180f;
			myTransform.eulerAngles = eulerAngles;
			yield return new WaitForSeconds(0.04f);
		}
	}

	private IEnumerator attackWithTentacles()
	{
		while (currentTarget != null)
		{
			yield return new WaitForSeconds(3f);
			if (leftFrontTentacle != null)
			{
				leftFrontTentacle.attackFromGround();
			}
			yield return new WaitForSeconds(1f);
			if (rightFrontTentacle != null)
			{
				rightFrontTentacle.attackFromGround();
			}
		}
	}

	private IEnumerator OnBodyDamaged()
	{
		if (isHanging)
		{
			bodyAnimation["MainBodyDamaged"].wrapMode = WrapMode.Once;
			bodyAnimation.CrossFade("MainBodyDamaged");
			yield return new WaitForSeconds(bodyAnimation["MainBodyDamaged"].length);
			bodyAnimation["MainBodyHangingIdle"].wrapMode = WrapMode.Loop;
			bodyAnimation.CrossFade("MainBodyHangingIdle");
			currentHP = maxHP;
		}
	}

	public void OnTentacleAttached()
	{
		if (isHanging)
		{
			numTentaclesLost--;
		}
	}
}
