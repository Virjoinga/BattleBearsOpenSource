using UnityEngine;

public abstract class ProjectileController : MonoBehaviour
{
	protected RangedWeaponController weapon;

	protected virtual void Awake()
	{
		weapon = GetComponent(typeof(RangedWeaponController)) as RangedWeaponController;
	}

	public abstract void FireProjectile(Vector3 dir);
}
