using System.Collections;
using UnityEngine;

public class OliverController : PlayerController
{
	public GameObject katanaPrefab;

	public GameObject uberKatanaPrefab;

	public GameObject katanaCollider;

	public MeleeWeaponController katanaController;

	public int[] katanaAmmoGains;

	private bool temporaryDisable;

	private GameObject katanaTrail;

	public AudioClip[] katanaAttackSounds;

	public AudioClip katanaArmSound;

	private bool swinging;

	private Vector3 origPos;

	protected override void Start()
	{
		base.Start();
		katanaController.disabled = true;
		temporaryDisable = false;
		origPos = katanaCollider.transform.localPosition;
	}

	public override void handleDying()
	{
		katanaCollider.active = false;
		katanaController.disabled = true;
		StopCoroutine("delayedKatanaOff");
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
		WeaponType weaponType = currentWeaponType;
		if (weaponType == WeaponType.KATANA && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
		{
			katanaTrail.active = true;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (currentWeaponType != WeaponType.KATANA)
		{
			return;
		}
		if (shoot)
		{
			if (!temporaryDisable)
			{
				katanaCollider.active = true;
				katanaController.disabled = false;
				if (GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
				{
					katanaTrail.active = true;
				}
				StartCoroutine("delayedKatanaOff");
			}
			if (!swinging)
			{
				StartCoroutine(katanaSwingLoop());
			}
		}
		else if (katanaTrail != null)
		{
			katanaTrail.active = false;
		}
	}

	public override float getSpecialAmmoGains()
	{
		return katanaAmmoGains[GameManager.Instance.getDifficulty()];
	}

	public override float getSpecialAmmoGains2()
	{
		return katanaAmmoGains[GameManager.Instance.getDifficulty()];
	}

	private IEnumerator katanaSwingLoop()
	{
		swinging = true;
		while (shoot && currentWeaponType == WeaponType.KATANA)
		{
			StatsManager.Instance.currentShotsFired++;
			AudioClip audioClip = katanaAttackSounds[Random.Range(0, katanaAttackSounds.Length)];
			SoundManager.Instance.playSound(audioClip);
			yield return new WaitForSeconds(audioClip.length * 2f);
		}
		swinging = false;
	}

	public override void switchToWeaponType(WeaponType newWeaponType, bool isSpecial)
	{
		base.switchToWeaponType(newWeaponType, isSpecial);
		currentWeaponType = newWeaponType;
		WeaponType weaponType = currentWeaponType;
		if (weaponType == WeaponType.KATANA)
		{
			if (isSpecial)
			{
				mountWeapon(uberKatanaPrefab, isSpecial);
			}
			else
			{
				mountWeapon(katanaPrefab, isSpecial);
			}
		}
	}

	protected override void mountWeapon(GameObject weapon, bool isSpecial)
	{
		base.mountWeapon(weapon, isSpecial);
		WeaponType weaponType = currentWeaponType;
		if (weaponType == WeaponType.KATANA)
		{
			currentWeapon.name = "Katana";
			katanaCollider.active = true;
			BoxCollider boxCollider = katanaCollider.GetComponent(typeof(BoxCollider)) as BoxCollider;
			if (isSpecial)
			{
				boxCollider.center = new Vector3(2f, -0.25f, 0f);
				boxCollider.size = new Vector3(6f, 2.5f, 7f);
			}
			else
			{
				boxCollider.center = new Vector3(0f, -0.25f, 0f);
				boxCollider.size = new Vector3(4f, 2.5f, 5f);
			}
			SoundManager.Instance.playSound(katanaArmSound);
			katanaTrail = currentWeapon.transform.Find("trail").gameObject;
			katanaTrail.active = false;
			katanaController.disabled = true;
			temporaryDisable = false;
		}
		else
		{
			temporaryDisable = false;
			katanaCollider.active = false;
			StopCoroutine("delayedKatanaOff");
		}
	}

	private IEnumerator delayedKatanaOff()
	{
		temporaryDisable = true;
		Transform katanaTransform = katanaCollider.transform;
		katanaTransform.localPosition = new Vector3(10000f, 10000f, 10000f);
		yield return new WaitForSeconds(0.1f);
		katanaTransform.localPosition = origPos;
		yield return new WaitForSeconds(0.25f);
		temporaryDisable = false;
		katanaController.disabled = true;
	}

	public override void OnGetSpecial2()
	{
	}

	public override void OnGetSpecial()
	{
		HUDController.Instance.addAmmo(WeaponType.KATANA, katanaAmmoGains[GameManager.Instance.getDifficulty()]);
	}
}
