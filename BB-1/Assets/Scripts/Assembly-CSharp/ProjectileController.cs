using UnityEngine;

public abstract class ProjectileController : MonoBehaviour
{
	protected RangedWeaponController weapon;

	protected Transform myTransform;

	protected virtual void Awake()
	{
		myTransform = base.transform;
		weapon = GetComponent(typeof(RangedWeaponController)) as RangedWeaponController;
	}

	public abstract void FireProjectile(Transform rootObj, int numShots);
}
