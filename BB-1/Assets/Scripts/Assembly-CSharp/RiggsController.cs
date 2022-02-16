using System.Collections;
using UnityEngine;

public class RiggsController : PlayerController
{
	public GameObject laserPrefab;

	public LaserController laser;

	public ParticleSystem laserBarrelEffectPS;

	private bool startingLaserFire;

	private bool laserFiring;

	public float[] laserTimeGains;

	public AudioClip laserPickupSound;

	public AudioClip laserPowerDownSound;

	public AudioClip laserFiringSound;

	private int startHuggablesKilled;

	public GameObject chainsawPrefab;

	public GameObject uberChainsawPrefab;

	public GameObject chainsawCollider;

	public MeleeWeaponController chainsawController;

	public int[] chainsawAmmoGains;

	private bool temporaryDisable;

	private GameObject chainsawTrail;

	public AudioClip[] chainsawAttackSounds;

	public AudioClip chainsawArmSound;

	private AudioSource chainsawIdle;

	public AudioClip chainsawWindDown;

	private bool swinging;

	private Vector3 origPos;

	public GameObject thirdPersonCam;

	public GameObject fpsCam;

	public Transform bulletTracer;

	private bool[] hasDoubleTapped = new bool[5];

	public AudioSource laserAudio;

	public LayerMask layerMask;

	protected override void Start()
	{
		base.Start();
		startingLaserFire = false;
		laserFiring = false;
		ParticleSystem.EmissionModule emission = laserBarrelEffectPS.emission;
		emission.enabled = false;
		if (GameManager.Instance.currentStage <= 2)
		{
			chainsawController.disabled = true;
		}
		temporaryDisable = false;
		origPos = chainsawCollider.transform.localPosition;
		chainsawCollider.SetActive(false);
		thirdPersonCam.SetActive(true);
		fpsCam.SetActive(false);
		for (int i = 0; i < 5; i++)
		{
			hasDoubleTapped[i] = false;
		}
		chainsawIdle = chainsawCollider.GetComponent(typeof(AudioSource)) as AudioSource;
	}

	public override void handleDying()
	{
		chainsawCollider.SetActive(false);
		chainsawController.disabled = true;
		StopCoroutine("delayedChainsawOff");
	}

	public override void saveState(Vector3 roomPosition, Vector2 cellPos, string hashString)
	{
		if (!isDying)
		{
			base.saveState(roomPosition, cellPos, hashString);
		}
	}

	protected override void loadState()
	{
		base.loadState();
		switch (currentWeaponType)
		{
		case WeaponType.LASER:
			laserFiring = false;
			break;
		case WeaponType.CHAINSAW:
			if (GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
			{
				chainsawTrail.SetActive(true);
			}
			break;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (currentWeaponType == WeaponType.CHAINSAW)
		{
			if (shoot)
			{
				if (!temporaryDisable)
				{
					chainsawCollider.SetActive(true);
					chainsawController.disabled = false;
					if (GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
					{
						chainsawTrail.SetActive(true);
					}
					StartCoroutine("delayedChainsawOff");
				}
				if (!swinging)
				{
					StartCoroutine(chainsawSwingLoop());
				}
			}
			else if (chainsawTrail != null)
			{
				chainsawTrail.SetActive(false);
			}
		}
		else if (shoot && currentWeaponType == WeaponType.LASER && !isDying)
		{
			if (!startingLaserFire)
			{
				startingLaserFire = true;
				StatsManager.Instance.currentShotsFired++;
				StartCoroutine(delayedLaserFire());
				startHuggablesKilled = StatsManager.Instance.totalHuggablesKilled;
			}
			if (laserFiring)
			{
				laser.Fire(Time.deltaTime);
				if (currentSatelliteController != null)
				{
					currentSatelliteController.shootLaser(weaponRot.localEulerAngles, Time.deltaTime);
				}
			}
		}
		else if (laserFiring)
		{
			int num = StatsManager.Instance.totalHuggablesKilled - startHuggablesKilled;
			int num2 = 30;
			laserAudio.Stop();
			SoundManager.Instance.playSound(laserPowerDownSound);
			startingLaserFire = false;
			laserFiring = false;
			ParticleSystem.EmissionModule emission = laserBarrelEffectPS.emission;
			emission.enabled = false;
			laser.toggleBeams(false);
			if (currentSatelliteController != null)
			{
				currentSatelliteController.turnOffLaser();
			}
		}
	}

	private IEnumerator delayedLaserFire()
	{
		yield return new WaitForSeconds(0.4f);
		laserAudio.volume = SoundManager.Instance.getEffectsVolume();
		laserAudio.clip = laserFiringSound;
		laserAudio.loop = true;
		laserAudio.Play();
		laserFiring = true;
		ParticleSystem.EmissionModule emission = laserBarrelEffectPS.emission;
		emission.enabled = true;
		laser.Fire(Time.deltaTime);
		laser.toggleBeams(true);
	}

	public override float getSpecialAmmoGains()
	{
		return laserTimeGains[GameManager.Instance.getDifficulty()];
	}

	public override float getSpecialAmmoGains2()
	{
		return chainsawAmmoGains[GameManager.Instance.getDifficulty()];
	}

	private IEnumerator chainsawSwingLoop()
	{
		swinging = true;
		while (shoot && currentWeaponType == WeaponType.CHAINSAW)
		{
			StatsManager.Instance.currentShotsFired++;
			AudioClip audioClip = chainsawAttackSounds[Random.Range(0, chainsawAttackSounds.Length)];
			SoundManager.Instance.playSound(audioClip);
			yield return new WaitForSeconds(audioClip.length);
		}
		SoundManager.Instance.playSound(chainsawWindDown);
		swinging = false;
	}

	public override void switchToWeaponType(WeaponType newWeaponType, bool isSpecial)
	{
		base.switchToWeaponType(newWeaponType, isSpecial);
		currentWeaponType = newWeaponType;
		switch (currentWeaponType)
		{
		case WeaponType.LASER:
			mountWeapon(laserPrefab, isSpecial);
			break;
		case WeaponType.CHAINSAW:
			if (isSpecial)
			{
				mountWeapon(uberChainsawPrefab, isSpecial);
			}
			else
			{
				mountWeapon(chainsawPrefab, isSpecial);
			}
			StartCoroutine(chainSawIdleSound());
			break;
		}
	}

	public IEnumerator chainSawIdleSound()
	{
		yield return new WaitForSeconds(1.875f);
		chainsawIdle.volume = SoundManager.Instance.getEffectsVolume();
		chainsawIdle.loop = true;
		chainsawIdle.Play();
	}

	protected override void mountWeapon(GameObject weapon, bool isSpecial)
	{
		base.mountWeapon(weapon, isSpecial);
		switch (currentWeaponType)
		{
		case WeaponType.LASER:
			weaponTransform = laser.transform;
			SoundManager.Instance.playSound(laserPickupSound);
			laser.numBounces = (isSpecial ? 10 : 0);
			if (isSpecial && laserFiring)
			{
				laser.toggleBeams(false);
				laser.toggleBeams(true);
			}
			break;
		case WeaponType.CHAINSAW:
		{
			currentWeapon.name = "Chainsaw";
			chainsawCollider.SetActive(true);
			BoxCollider boxCollider = chainsawCollider.GetComponent(typeof(BoxCollider)) as BoxCollider;
			if (isSpecial)
			{
				boxCollider.center = new Vector3(-2f, -0.25f, 0f);
				boxCollider.size = new Vector3(6f, 2.5f, 7f);
			}
			else
			{
				boxCollider.center = new Vector3(0f, -0.25f, 0f);
				boxCollider.size = new Vector3(4f, 2.5f, 5f);
			}
			SoundManager.Instance.playSound(chainsawArmSound);
			chainsawTrail = currentWeapon.transform.Find("trail").gameObject;
			chainsawTrail.SetActive(false);
			chainsawController.disabled = true;
			temporaryDisable = false;
			break;
		}
		default:
			temporaryDisable = false;
			chainsawCollider.SetActive(false);
			StopCoroutine("delayedChainsawOff");
			break;
		}
	}

	private IEnumerator delayedChainsawOff()
	{
		temporaryDisable = true;
		Transform chainsawTransform = chainsawCollider.transform;
		chainsawTransform.localPosition = new Vector3(10000f, 10000f, 10000f);
		yield return new WaitForSeconds(0.1f);
		chainsawTransform.localPosition = origPos;
		yield return new WaitForSeconds(0.25f);
		temporaryDisable = false;
		chainsawController.disabled = true;
	}

	public override void OnGetSpecial()
	{
		if (GameManager.Instance.currentStage < 3 && GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
		{
			HUDController.Instance.addAmmo(WeaponType.LASER, laserTimeGains[GameManager.Instance.getDifficulty()]);
		}
		else if (Random.value < 0.25f)
		{
			HUDController.Instance.addAmmo(WeaponType.LASER, laserTimeGains[GameManager.Instance.getDifficulty()]);
		}
		else
		{
			HUDController.Instance.addAmmo(WeaponType.CHAINSAW, chainsawAmmoGains[GameManager.Instance.getDifficulty()]);
		}
	}

	public override void OnGetSpecial2()
	{
		StartCoroutine(delayedChainsawSwitch());
	}

	private IEnumerator delayedChainsawSwitch()
	{
		yield return new WaitForSeconds(1f);
		HUDController.Instance.addAmmo(WeaponType.CHAINSAW, chainsawAmmoGains[GameManager.Instance.getDifficulty()]);
		HUDController.Instance.switchToWeapon(5);
	}
}
