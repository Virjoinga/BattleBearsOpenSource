using UnityEngine;

public class RangedWeaponController : MonoBehaviour
{
	public int numberOfBullets = 1;

	private float lastShotTime;

	public float shootDurration;

	public bool unlimitedAmmo;

	public bool isSpecial;

	private ProjectileController projectile;

	private void Start()
	{
		projectile = GetComponent(typeof(ProjectileController)) as ProjectileController;
	}

	public bool Shoot(Vector3 dir)
	{
		if (projectile == null)
		{
			return false;
		}
		if (Time.time - lastShotTime > shootDurration && unlimitedAmmo)
		{
			projectile.FireProjectile(dir);
			lastShotTime = Time.time;
			return true;
		}
		return false;
	}
}
