using System.Collections;
using UnityEngine;

public abstract class PlayerController : GenericCharacterController
{
	protected WeaponType currentWeaponType;

	public RangedWeaponController machinegunShooter;

	public RangedWeaponController spreadshotShooter;

	public RangedWeaponController bearzookaShooter;

	public RangedWeaponController specialSpreadshotShooter;

	public RangedWeaponController specialBearzookaShooter;

	private RangedWeaponController currentSpreadshotShooter;

	private RangedWeaponController currentBearzookaShooter;

	private Transform machinegunTransform;

	private Transform spreadshotTransform;

	private Transform bearzookaTransform;

	private GameObject machinegunGameObject;

	private GameObject spreadshotGameObject;

	private GameObject bearzookaGameObject;

	private AudioSource machinegunAudio;

	public Transform gunMountpoint;

	public GameObject machinegun;

	protected GameObject currentWeapon;

	protected BearAnimator animator;

	private DamageReceiver dmgReceiver;

	public int maxHealth = 250;

	private Vector3 currentSpawnPoint;

	private Quaternion currentSpawnRotation;

	public int numLives = 3;

	protected bool isDying;

	public SatelliteController currentSatelliteController;

	protected GameObject currentShield;

	private Renderer currentShieldRenderer;

	public GameObject spreadshotPrefab;

	public GameObject bearzookaPrefab;

	public GameObject satellitePrefab;

	public GameObject shieldPrefab;

	public AudioClip shieldExpired;

	public AudioClip bearzookaFire;

	public AudioClip spreadshotFire;

	public AudioClip machinegunFire;

	public AudioClip walkSlow;

	public AudioClip walkFast;

	public AudioClip shieldActive;

	private bool hasCoffee;

	public int[] spreadshotAmmoGains;

	public int[] bearzookaAmmoGains;

	public float[] shieldDurations;

	public float[] coffeeDurations;

	public int[] foodGains;

	public AudioClip deathSound;

	private bool firingMachinegun;

	private bool hurtSound;

	public AudioClip[] hurtSounds;

	public float damageBeforeBattlecry = 100f;

	private float currentBattlecryDamage;

	public string battlecriesDirectory;

	private AudioClip[] battlecries;

	public ParticleSystem superWeaponEmitterTPSPS;

	public Transform weaponRot;

	private bool startingFiringMachinegun;

	private float machinegunStartFireTime;

	private bool startingFiringSpreadshot;

	private float spreadshotStartFireTime;

	public GameObject alwaysShotgun;

	public BearAnimator getAnimator()
	{
		return animator;
	}

	public abstract void OnGetSpecial();

	public abstract void OnGetSpecial2();

	public abstract void handleDying();

	public abstract float getSpecialAmmoGains();

	public abstract float getSpecialAmmoGains2();

	public override void startShooting()
	{
		shoot = true;
		HUDController.Instance.OnSetFireReticle();
	}

	public override void stopShooting()
	{
		shoot = false;
		HUDController.Instance.OnSetNonFireReticle();
	}

	public virtual void OnGetBearzooka()
	{
		HUDController.Instance.addAmmo(WeaponType.BEARZOOKA, bearzookaAmmoGains[GameManager.Instance.getDifficulty()]);
	}

	public virtual void OnGetSpreadshot()
	{
		HUDController.Instance.addAmmo(WeaponType.SPREADSHOT, spreadshotAmmoGains[GameManager.Instance.getDifficulty()]);
	}

	protected override void Awake()
	{
		base.Awake();
		machinegunTransform = machinegunShooter.gameObject.transform;
		spreadshotTransform = spreadshotShooter.gameObject.transform;
		bearzookaTransform = bearzookaShooter.gameObject.transform;
		if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			machinegunTransform.position = new Vector3(machinegunTransform.position.x - 1f, machinegunTransform.position.y - 1f, machinegunTransform.position.z);
			spreadshotTransform.position = new Vector3(spreadshotTransform.position.x - 1f, spreadshotTransform.position.y - 1f, spreadshotTransform.position.z);
		}
		machinegunGameObject = machinegunShooter.gameObject;
		spreadshotGameObject = spreadshotShooter.gameObject;
		bearzookaGameObject = bearzookaShooter.gameObject;
		weaponTransform = machinegunTransform;
	}

	protected override void Start()
	{
		base.Start();
		machinegunAudio = machinegunGameObject.GetComponent(typeof(AudioSource)) as AudioSource;
		animator = GetComponentInChildren(typeof(BearAnimator)) as BearAnimator;
		dmgReceiver = GetComponentInChildren(typeof(DamageReceiver)) as DamageReceiver;
		dmgReceiver.hitpoints = maxHealth;
		characterController.detectCollisions = false;
		HUDController.Instance.linkupPlayer();
		if (GameManager.Instance.isLoading || GameManager.Instance.isGoingUpElevator)
		{
			loadState();
		}
		else
		{
			HUDController.Instance.switchToWeapon((GameManager.Instance.currentCharacter == Character.WIL) ? 6 : 0);
			HUDController.Instance.forceWeaponToggleHide();
		}
		if (GameManager.Instance.currentGameMode != GameMode.SURVIVAL)
		{
			HUDController.Instance.updateLives();
		}
		currentSpawnPoint = myTransform.position;
		dmgReceiver.listeners.Add(base.gameObject);
		HUDController.Instance.OnNewHealth(dmgReceiver.hitpoints);
		if (GameManager.Instance.useHighres && battlecriesDirectory != "" && !GameManager.Instance.vent)
		{
			loadBattlecries();
		}
	}

	private void loadBattlecries()
	{
		Object[] array = Resources.LoadAll("Music/High/" + battlecriesDirectory, typeof(AudioClip));
		if (array.Length != 0 && !GameManager.Instance.vent)
		{
			battlecries = new AudioClip[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				battlecries[i] = array[i] as AudioClip;
			}
		}
	}

	public virtual void saveState(Vector3 roomPosition, Vector2 cellPos, string hashString)
	{
		if (!isDying)
		{
			SavedDataManager.Instance.saveState();
			StatsManager.Instance.OnSave();
			PlayerPrefs.SetFloat("hitpoints", dmgReceiver.hitpoints);
			PlayerPrefs.SetInt("lives", numLives);
			PlayerPrefs.SetInt("satellite", (currentSatelliteController != null) ? 1 : 0);
			PlayerPrefs.SetInt("currentWeapon", (int)currentWeaponType);
			PlayerPrefs.SetFloat("spreadshotAmmo", HUDController.Instance.currentAmmoAmounts[1]);
			PlayerPrefs.SetFloat("bearzookaAmmo", HUDController.Instance.currentAmmoAmounts[2]);
			PlayerPrefs.SetFloat("katanaAmmo", HUDController.Instance.currentAmmoAmounts[3]);
			PlayerPrefs.SetFloat("laserAmmo", HUDController.Instance.currentAmmoAmounts[4]);
			PlayerPrefs.SetFloat("chainsawAmmo", HUDController.Instance.currentAmmoAmounts[5]);
			PlayerPrefs.SetFloat("shotgunAmmo", HUDController.Instance.currentAmmoAmounts[7]);
			string text = "";
			for (int i = 0; i < HUDController.Instance.specialModes.Length; i++)
			{
				text += (HUDController.Instance.specialModes[i] ? "1" : "0");
			}
			PlayerPrefs.SetString("specialModes", text);
			PlayerPrefs.SetFloat("playerX", myTransform.position.x);
			PlayerPrefs.SetFloat("playerY", myTransform.position.y);
			PlayerPrefs.SetFloat("playerZ", myTransform.position.z);
			PlayerPrefs.SetFloat("playerRotX", myTransform.eulerAngles.x);
			PlayerPrefs.SetFloat("playerRotY", myTransform.eulerAngles.y);
			PlayerPrefs.SetFloat("playerRotZ", myTransform.eulerAngles.z);
			PlayerPrefs.SetFloat("roomX", roomPosition.x);
			PlayerPrefs.SetFloat("roomY", roomPosition.y);
			PlayerPrefs.SetFloat("roomZ", roomPosition.z);
			PlayerPrefs.SetFloat("cellX", cellPos.x);
			PlayerPrefs.SetFloat("cellY", cellPos.y);
			PlayerPrefs.SetString("clearedRooms", hashString);
			if (HUDController.Instance != null)
			{
				HUDController.Instance.OnSaveMinimap();
			}
		}
	}

	protected virtual void loadState()
	{
		StatsManager.Instance.OnLoad();
		dmgReceiver.hitpoints = PlayerPrefs.GetFloat("hitpoints", maxHealth);
		numLives = PlayerPrefs.GetInt("lives", 3);
		if (PlayerPrefs.GetInt("satellite", 0) == 1)
		{
			OnAddSatellite();
		}
		int @int = PlayerPrefs.GetInt("currentWeapon", 0);
		HUDController.Instance.currentAmmoAmounts[1] = PlayerPrefs.GetFloat("spreadshotAmmo", 0f);
		HUDController.Instance.currentAmmoAmounts[2] = PlayerPrefs.GetFloat("bearzookaAmmo", 0f);
		HUDController.Instance.currentAmmoAmounts[3] = PlayerPrefs.GetFloat("katanaAmmo", 0f);
		HUDController.Instance.currentAmmoAmounts[4] = PlayerPrefs.GetFloat("laserAmmo", 0f);
		HUDController.Instance.currentAmmoAmounts[5] = PlayerPrefs.GetFloat("chainsawAmmo", 0f);
		HUDController.Instance.currentAmmoAmounts[7] = PlayerPrefs.GetFloat("shotgunAmmo", 0f);
		currentWeaponType = (WeaponType)@int;
		string @string = PlayerPrefs.GetString("specialModes", "");
		if (@string.Length == HUDController.Instance.specialModes.Length)
		{
			for (int i = 0; i < HUDController.Instance.specialModes.Length; i++)
			{
				HUDController.Instance.specialModes[i] = @string[i] == '1';
			}
		}
		HUDController.Instance.switchToWeapon(@int);
		if (!GameManager.Instance.isGoingUpElevator)
		{
			float @float = PlayerPrefs.GetFloat("playerX");
			float float2 = PlayerPrefs.GetFloat("playerY");
			float float3 = PlayerPrefs.GetFloat("playerZ");
			float float4 = PlayerPrefs.GetFloat("playerRotX");
			float float5 = PlayerPrefs.GetFloat("playerRotY");
			float float6 = PlayerPrefs.GetFloat("playerRotZ");
			myTransform.position = new Vector3(@float, float2, float3);
			myTransform.eulerAngles = new Vector3(float4, float5, float6);
		}
	}

	private IEnumerator playDeathSequence(bool falling)
	{
		handleDying();
		isDying = true;
		StatsManager.Instance.currentDeaths++;
		dmgReceiver.isInvincible = true;
		HUDController.Instance.OnNewHealth(0f);
		numLives--;
		if (deathSound != null)
		{
			SoundManager.Instance.playSound(deathSound);
		}
		if (falling)
		{
			StatsManager.Instance.currentFalls++;
			int currentFall = StatsManager.Instance.currentFalls;
			int num = 3;
		}
		if (currentSatelliteController != null)
		{
			StatsManager.Instance.numberOfRoomsWithSatellite = 0;
			currentSatelliteController.gameObject.SendMessage("OnObjectDestroyed");
			Object.Destroy(currentSatelliteController.gameObject);
		}
		if (numLives <= 0)
		{
			PlayerPrefs.DeleteKey("character");
		}
		if (falling)
		{
			yield return StartCoroutine(animator.playFallAnimation());
		}
		else
		{
			yield return StartCoroutine(animator.playDeathAnimation());
		}
		HUDController.Instance.updateLives();
		if (numLives <= 0)
		{
			if (StatsManager.Instance.currentRoomsVisited == 0)
			{
				GameMode currentGameMode = GameManager.Instance.currentGameMode;
			}
			yield return new WaitForSeconds(2.5f);
			if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
			{
				if (GameManager.Instance.currentCharacter == Character.OLIVER)
				{
					if (GameManager.Instance.inOliverBossRoom)
					{
						GameManager.Instance.PlayMovie("o_boss1_loss");
					}
					else
					{
						GameManager.Instance.PlayMovie("o_loss_standard");
					}
				}
				else if (GameManager.Instance.currentCharacter == Character.RIGGS)
				{
					if (GameManager.Instance.inRiggsBossRoom)
					{
						if (GameManager.Instance.inUdderMode)
						{
							GameManager.Instance.PlayMovie("r_boss1udder_loss");
						}
						else
						{
							GameManager.Instance.PlayMovie("r_boss1_loss");
						}
					}
					else
					{
						GameManager.Instance.PlayMovie("r_loss_standard");
					}
				}
				else if (GameManager.Instance.currentCharacter == Character.WIL)
				{
					GameManager.Instance.PlayMovie("w_loss");
				}
				else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
				{
					if (GameManager.Instance.currentCharacter == Character.RIGGS)
					{
						GameManager.Instance.PlayMovie("r_loss_standard");
					}
					else if (GameManager.Instance.currentCharacter == Character.OLIVER)
					{
						GameManager.Instance.PlayMovie("o_loss_standard");
					}
					else
					{
						GameManager.Instance.PlayMovie("w_loss");
					}
				}
			}
			StatsManager.Instance.currentPlayTime += Time.timeSinceLevelLoad;
			yield return new WaitForSeconds(0.5f);
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
		else
		{
			OnGetShield();
			StopCoroutine("delayedCoffeeExpiry");
			hasCoffee = false;
			currentMoveSpeed = moveSpeed;
			myTransform.position = currentSpawnPoint;
			myTransform.rotation = currentSpawnRotation;
			dmgReceiver.hitpoints = maxHealth;
			HUDController.Instance.resetWeapons();
			HUDController.Instance.OnNewHealth(maxHealth);
			if (GameManager.Instance.hasAcquiredSpecial && GameManager.Instance.currentCharacter == Character.WIL)
			{
				GameObject obj = Object.Instantiate(alwaysShotgun);
				obj.transform.parent = base.transform.parent;
				obj.transform.position = base.transform.position;
				obj.transform.rotation = base.transform.rotation;
				obj.name = alwaysShotgun.name;
			}
		}
		isDying = false;
		Vector3 vector = new Vector3(0f, 0f, 0.11f);
		animator.animateBear(vector, false);
	}

	public void OnDamageTaken(float amt)
	{
		if (battlecries != null && battlecries.Length != 0)
		{
			currentBattlecryDamage += amt;
			if (currentBattlecryDamage > damageBeforeBattlecry)
			{
				currentBattlecryDamage = 0f;
				SoundManager.Instance.playSound(battlecries[Random.Range(0, battlecries.Length)]);
			}
		}
	}

	public void OnNewHealth(float newHealth)
	{
		if (currentSatelliteController != null)
		{
			StatsManager.Instance.numberOfRoomsWithSatellite = 0;
			currentSatelliteController.gameObject.SendMessage("OnObjectDestroyed");
			Object.Destroy(currentSatelliteController.gameObject);
		}
		if (!hurtSound)
		{
			StartCoroutine(playHurtSound());
		}
		StatsManager.Instance.wasHit = 1;
	}

	private IEnumerator playHurtSound()
	{
		hurtSound = true;
		SoundManager.Instance.playSound(hurtSounds[Random.Range(0, hurtSounds.Length)]);
		yield return new WaitForSeconds(1f);
		hurtSound = false;
	}

	public void OnObjectDestroyed()
	{
		if (!isDying)
		{
			StartCoroutine(playDeathSequence(false));
		}
	}

	public void OnSetSpawn(Transform spawn)
	{
		currentSpawnPoint = spawn.position;
		currentSpawnRotation = spawn.rotation;
	}

	public void OnGetExtraLife()
	{
		numLives++;
		HUDController.Instance.updateLives();
	}

	public void OnAddSatellite()
	{
		if (currentSatelliteController != null)
		{
			Object.Destroy(currentSatelliteController.gameObject);
		}
		GameObject gameObject = Object.Instantiate(satellitePrefab);
		currentSatelliteController = gameObject.GetComponent(typeof(SatelliteController)) as SatelliteController;
	}

	public void OnAddFood()
	{
		dmgReceiver.hitpoints += foodGains[GameManager.Instance.getDifficulty()];
		if (dmgReceiver.hitpoints > (float)maxHealth)
		{
			dmgReceiver.hitpoints = maxHealth;
		}
		HUDController.Instance.OnNewHealth(dmgReceiver.hitpoints);
	}

	public void OnScreenClear(int amount)
	{
		int count = HuggableController.currentBaddies.Count;
		int num = 20;
		ModeManager.Instance.getCurrentRoom().BroadcastMessage("OnHit", amount, SendMessageOptions.DontRequireReceiver);
	}

	public void OnGetShield()
	{
		if (currentShield == null)
		{
			currentShield = Object.Instantiate(shieldPrefab);
			currentShieldRenderer = currentShield.GetComponent<Renderer>();
			CameraFollow componentInChildren = GetComponentInChildren<CameraFollow>();
			if (componentInChildren != null)
			{
				componentInChildren.SetShieldObj(currentShield);
			}
		}
		dmgReceiver.isInvincible = true;
		myAudio.volume = SoundManager.Instance.getEffectsVolume();
		myAudio.clip = shieldActive;
		myAudio.loop = true;
		myAudio.Play();
		StopCoroutine("delayedShieldExpiry");
		StartCoroutine("delayedShieldExpiry", shieldDurations[GameManager.Instance.getDifficulty()]);
	}

	private IEnumerator delayedShieldExpiry(int duration)
	{
		yield return new WaitForSeconds((float)duration - 3f);
		float timeLeft = 3f;
		bool toggle = false;
		while (timeLeft > 0f)
		{
			Color color = currentShieldRenderer.material.color;
			if (toggle)
			{
				color.a = 0.25f;
			}
			else
			{
				color.a = 0.5f;
			}
			currentShieldRenderer.material.color = color;
			toggle = !toggle;
			timeLeft -= 0.05f;
			yield return new WaitForSeconds(0.05f);
		}
		if (currentShield != null)
		{
			Object.Destroy(currentShield);
		}
		SoundManager.Instance.playSound(shieldExpired);
		dmgReceiver.isInvincible = false;
		myAudio.Stop();
	}

	public void OnDrinkCoffee()
	{
		hasCoffee = true;
		currentMoveSpeed = moveSpeed * 2f;
		StopCoroutine("delayedCoffeeExpiry");
		StartCoroutine("delayedCoffeeExpiry", coffeeDurations[GameManager.Instance.getDifficulty()]);
	}

	private IEnumerator delayedCoffeeExpiry(int duration)
	{
		yield return new WaitForSeconds(duration);
		hasCoffee = false;
		currentMoveSpeed = moveSpeed;
	}

	public virtual void switchToWeaponType(WeaponType newWeaponType, bool isSpecial)
	{
		currentWeaponType = newWeaponType;
		switch (currentWeaponType)
		{
		case WeaponType.MACHINEGUN:
			mountWeapon(machinegun, isSpecial);
			break;
		case WeaponType.SPREADSHOT:
			mountWeapon(spreadshotPrefab, isSpecial);
			break;
		case WeaponType.BEARZOOKA:
			mountWeapon(bearzookaPrefab, isSpecial);
			break;
		}
	}

	protected virtual void mountWeapon(GameObject weapon, bool isSpecial)
	{
		ParticleSystem.EmissionModule emission = superWeaponEmitterTPSPS.emission;
		emission.enabled = isSpecial;
		Object.Destroy(currentWeapon);
		GameObject gameObject = Object.Instantiate(weapon);
		gameObject.transform.parent = gunMountpoint;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = ((weapon.name == "Shotgun") ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 90f, 0f));
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.name = weapon.name;
		currentWeapon = gameObject;
		animator.setWeapon(currentWeapon);
		spreadshotShooter.enabled = false;
		specialSpreadshotShooter.enabled = false;
		bearzookaShooter.gameObject.SetActive(false);
		specialBearzookaShooter.gameObject.SetActive(false);
		machinegunShooter.gameObject.SetActive(false);
		if (firingMachinegun)
		{
			firingMachinegun = false;
			machinegunAudio.Stop();
		}
		if (currentWeaponType == WeaponType.SPREADSHOT)
		{
			spreadshotShooter.enabled = !isSpecial;
			specialSpreadshotShooter.enabled = isSpecial;
			currentSpreadshotShooter = (isSpecial ? specialSpreadshotShooter : spreadshotShooter);
			weaponTransform = spreadshotTransform;
		}
		else if (currentWeaponType == WeaponType.BEARZOOKA)
		{
			bearzookaShooter.gameObject.SetActive(!isSpecial);
			specialBearzookaShooter.gameObject.SetActive(isSpecial);
			currentBearzookaShooter = (isSpecial ? specialBearzookaShooter : bearzookaShooter);
			weaponTransform = bearzookaTransform;
		}
		else if (currentWeaponType == WeaponType.MACHINEGUN)
		{
			machinegunShooter.gameObject.SetActive(true);
			weaponTransform = machinegunTransform;
			firingMachinegun = false;
		}
	}

	protected virtual void Update()
	{
		if (!isDying && myTransform.position.y < -0.2f && !ModeManager.Instance.inMoveOutDoor)
		{
			StartCoroutine(playDeathSequence(true));
		}
		weaponRot = weaponTransform;
		if (inputDisabled)
		{
			return;
		}
		if (!isDying)
		{
			myTransform.Rotate(turn * Time.deltaTime);
		}
		bool flag = shoot;
		if (!shoot)
		{
			startingFiringMachinegun = false;
			startingFiringSpreadshot = false;
		}
		if (shoot && !isDying)
		{
			if (currentWeaponType == WeaponType.MACHINEGUN)
			{
				if (!startingFiringMachinegun)
				{
					startingFiringMachinegun = true;
					machinegunStartFireTime = Time.time;
				}
				if (startingFiringMachinegun && Time.time > machinegunStartFireTime + 0f)
				{
					if (!firingMachinegun)
					{
						firingMachinegun = true;
						machinegunAudio.volume = SoundManager.Instance.getEffectsVolume();
						machinegunAudio.clip = machinegunFire;
						machinegunAudio.loop = true;
						machinegunAudio.Play();
					}
					if (machinegunShooter.Shoot())
					{
						StatsManager.Instance.currentShotsFired++;
					}
				}
				if (currentSatelliteController != null)
				{
					currentSatelliteController.shootMachineGun(weaponRot.localEulerAngles);
				}
			}
			else if (currentWeaponType == WeaponType.SPREADSHOT)
			{
				if (!startingFiringSpreadshot)
				{
					startingFiringSpreadshot = true;
					spreadshotStartFireTime = Time.time;
				}
				if (startingFiringSpreadshot && Time.time > spreadshotStartFireTime + 0f && currentSpreadshotShooter.Shoot())
				{
					SoundManager.Instance.playSound(spreadshotFire);
					StatsManager.Instance.currentShotsFired++;
				}
				if (currentSatelliteController != null)
				{
					currentSatelliteController.shootSpreadshot(weaponRot.localEulerAngles);
				}
			}
			else if (currentWeaponType == WeaponType.BEARZOOKA)
			{
				flag = currentBearzookaShooter.Shoot();
				if (flag)
				{
					SoundManager.Instance.playSound(bearzookaFire);
					StatsManager.Instance.currentShotsFired++;
				}
				if (currentSatelliteController != null)
				{
					currentSatelliteController.shootBearzooka(weaponRot.localEulerAngles);
				}
			}
		}
		else if (firingMachinegun)
		{
			firingMachinegun = false;
			machinegunAudio.Stop();
		}
		if (!usingDirectAiming && !isDying)
		{
			weaponTransform.localEulerAngles = weaponDir;
		}
		if (isDying)
		{
			movement = Vector3.zero;
		}
		float magnitude = movement.magnitude;
		if (currentShield == null)
		{
			if (magnitude < 0.25f)
			{
				myAudio.Stop();
			}
			else if ((magnitude < 10f && lastMovementSpeed > 10f) || lastMovementSpeed < 2.5f)
			{
				myAudio.volume = SoundManager.Instance.getEffectsVolume();
				myAudio.clip = walkSlow;
				myAudio.Play();
			}
			else if (magnitude >= 10f && magnitude <= 20f && (lastMovementSpeed < 10f || lastMovementSpeed > 20f))
			{
				myAudio.volume = SoundManager.Instance.getEffectsVolume();
				myAudio.clip = walkFast;
				myAudio.Play();
			}
		}
		lastMovementSpeed = magnitude;
		characterController.SimpleMove(movement);
		if (!isDying)
		{
			animator.animateBear(movement, flag);
		}
	}

	public void OnAutoPilot()
	{
		inputDisabled = true;
	}

	public void OffAutoPilot()
	{
		inputDisabled = false;
	}
}
