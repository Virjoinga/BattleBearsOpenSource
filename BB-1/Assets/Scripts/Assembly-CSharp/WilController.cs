using System.Collections;
using UnityEngine;

public class WilController : PlayerController
{
	public GameObject arrowPrefab;

	public GameObject arrowCollider;

	public MeleeWeaponController arrowController;

	private bool temporaryDisable;

	public AudioClip[] arrowAttackSounds;

	public AudioClip arrowArmSound;

	public GameObject shotgunPrefab;

	public RangedWeaponController shotgunShoot;

	public AudioClip shotgunPickupSound;

	public AudioClip shotgunShootSound;

	public int[] shotgunAmmoGains;

	private bool swinging;

	public ParticleSystem dustPS;

	public GameObject light;

	protected override void Start()
	{
		base.Start();
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL || GameManager.Instance.currentStage >= 2)
		{
			GameObject obj = Object.Instantiate(alwaysShotgun);
			obj.transform.parent = base.transform.parent;
			obj.transform.position = base.transform.position;
			obj.transform.rotation = base.transform.rotation;
			obj.name = alwaysShotgun.name;
		}
		if (GameManager.Instance.vent)
		{
			dustPS.enableEmission = true;
			light.SetActive(true);
		}
		else
		{
			dustPS.enableEmission = false;
			light.SetActive(false);
		}
	}

	public override void handleDying()
	{
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
		if (Application.loadedLevelName == "WilCampaignLevel1")
		{
			HUDController.Instance.forceWeaponToggleHide();
		}
		GameObject obj = Object.Instantiate(alwaysShotgun);
		obj.transform.parent = base.transform.parent;
		obj.transform.position = base.transform.position;
		obj.transform.rotation = base.transform.rotation;
		obj.name = alwaysShotgun.name;
	}

	public override void switchToWeaponType(WeaponType newWeaponType, bool isSpecial)
	{
		base.switchToWeaponType(newWeaponType, false);
		currentWeaponType = newWeaponType;
		switch (currentWeaponType)
		{
		case WeaponType.ARROW:
			mountWeapon(arrowPrefab, false);
			break;
		case WeaponType.SHOTGUN:
			mountWeapon(shotgunPrefab, false);
			break;
		}
	}

	protected override void mountWeapon(GameObject weapon, bool isSpecial)
	{
		base.mountWeapon(weapon, false);
		switch (currentWeaponType)
		{
		case WeaponType.ARROW:
			currentWeapon.name = "Arrow";
			SoundManager.Instance.playSound(arrowArmSound);
			arrowController.gameObject.active = false;
			temporaryDisable = false;
			break;
		case WeaponType.SHOTGUN:
			currentWeapon.name = "Shotgun";
			weaponTransform = shotgunShoot.transform;
			SoundManager.Instance.playSound(shotgunPickupSound);
			break;
		default:
			temporaryDisable = false;
			arrowCollider.active = false;
			break;
		}
	}

	protected override void Update()
	{
		if (currentWeaponType == WeaponType.ARROW)
		{
			if (shoot && !isDying)
			{
				if (!temporaryDisable)
				{
					arrowCollider.active = true;
					arrowController.disabled = false;
				}
				if (!swinging)
				{
					StartCoroutine(arrowSwingLoop());
				}
			}
			else
			{
				arrowController.gameObject.active = false;
			}
		}
		if (shoot && currentWeaponType == WeaponType.SHOTGUN && !isDying)
		{
			if (shotgunShoot.Shoot())
			{
				SoundManager.Instance.playSound(shotgunShootSound);
				StatsManager.Instance.currentShotsFired++;
				if (currentSatelliteController != null)
				{
					currentSatelliteController.shootShotgun(weaponRot.localEulerAngles);
				}
				PlayerPrefs.SetInt("hasShotgunFired", 1);
			}
			else
			{
				shoot = false;
			}
		}
		base.Update();
	}

	private IEnumerator arrowSwingLoop()
	{
		swinging = true;
		while (shoot && currentWeaponType == WeaponType.ARROW)
		{
			StatsManager.Instance.currentShotsFired++;
			AudioClip audioClip = arrowAttackSounds[Random.Range(0, arrowAttackSounds.Length)];
			SoundManager.Instance.playSound(audioClip);
			yield return new WaitForSeconds(audioClip.length * 2f);
		}
		swinging = false;
	}

	public override float getSpecialAmmoGains()
	{
		return shotgunAmmoGains[GameManager.Instance.getDifficulty()];
	}

	public override float getSpecialAmmoGains2()
	{
		return shotgunAmmoGains[GameManager.Instance.getDifficulty()];
	}

	public override void OnGetSpecial2()
	{
		StartCoroutine(delayedShotgun());
	}

	public override void OnGetSpecial()
	{
		StartCoroutine(delayedShotgun());
	}

	public IEnumerator delayedShotgun()
	{
		yield return new WaitForSeconds(0.1f);
		HUDController.Instance.addAmmo(WeaponType.SHOTGUN, shotgunAmmoGains[GameManager.Instance.getDifficulty()]);
	}

	public void OnLevelWasLoaded()
	{
		if (Application.loadedLevelName == "WilCampaignLevel1" || Application.loadedLevelName == "WilCampaignLevel3")
		{
			GameManager.Instance.vent = true;
		}
		else
		{
			GameManager.Instance.vent = false;
		}
	}
}
