using UnityEngine;

public class RangedWeaponController : MonoBehaviour
{
	public int numBulletsToFire = 1;

	public float shotDuration;

	private Transform myTransform;

	private ProjectileController projectiles;

	private PlayerController playerController;

	private float lastShotTime;

	private float fireStartTime;

	public ParticleSystem particleEffectPS;

	public ParticleSystem particleEffectPS2;

	public WeaponType weaponType;

	public bool unlimitedAmmo;

	public bool isSpecial;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void Start()
	{
		projectiles = GetComponent(typeof(ProjectileController)) as ProjectileController;
		playerController = base.transform.root.gameObject.GetComponent(typeof(PlayerController)) as PlayerController;
	}

	public bool Shoot()
	{
		if (projectiles == null)
		{
			return false;
		}
		if (Time.time - lastShotTime > shotDuration && (HUDController.Instance.currentAmmoAmounts[(int)weaponType] > 0f || weaponType == WeaponType.MACHINEGUN || weaponType == WeaponType.SHOTGUN))
		{
			projectiles.FireProjectile(myTransform, numBulletsToFire);
			if (particleEffectPS != null && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
			{
				particleEffectPS.Emit(1);
				if (particleEffectPS2 != null)
				{
					particleEffectPS2.Emit(1);
				}
			}
			lastShotTime = Time.time;
			if (weaponType != 0 && !unlimitedAmmo)
			{
				HUDController.Instance.addAmmo(weaponType, -1f);
			}
			return true;
		}
		return false;
	}
}
