using UnityEngine;

public class SimpleRangedWeaponController : MonoBehaviour
{
	public int numBulletsToFire = 1;

	public float shotDuration;

	private Transform myTransform;

	private ProjectileController projectiles;

	private float lastShotTime;

	private float fireStartTime;

	public ParticleSystem particleEffectPS;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void Start()
	{
		projectiles = GetComponent(typeof(ProjectileController)) as ProjectileController;
	}

	public bool Shoot()
	{
		if (projectiles == null)
		{
			return false;
		}
		if (Time.time - lastShotTime > shotDuration * Time.timeScale)
		{
			projectiles.FireProjectile(myTransform, numBulletsToFire);
			if (particleEffectPS != null && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
			{
				particleEffectPS.Emit(1);
			}
			lastShotTime = Time.time;
			return true;
		}
		return false;
	}
}
