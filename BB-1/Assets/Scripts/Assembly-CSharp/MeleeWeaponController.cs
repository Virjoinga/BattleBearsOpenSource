using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
	public float maxAmmo = 20f;

	private float currentAmmo;

	private Transform myTransform;

	private PlayerController playerController;

	public ParticleSystem particleEffectPS;

	public bool unlimitedAmmo;

	public string[] colliderLayers;

	public bool disabled;

	public AudioClip hitSound;

	public AudioClip blockSound;

	public bool alwaysCutHead;

	public float damage;

	public bool isEnemyOwned;

	public WeaponType weaponType;

	public GameObject blood;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void Start()
	{
		playerController = base.transform.root.gameObject.GetComponent(typeof(PlayerController)) as PlayerController;
	}

	public float getAmmo()
	{
		return currentAmmo;
	}

	public void setMaxAmmo(float max)
	{
		maxAmmo = max;
	}

	public void setAmmo(float ammo)
	{
		currentAmmo = ammo;
	}

	public void OnBlock()
	{
		playerController.getAnimator().blockAnimation();
		SoundManager.Instance.playSound(blockSound);
		reduceAmmo();
	}

	public void OnTriggerEnter(Collider c)
	{
		if (!disabled)
		{
			handleCollision(c.gameObject);
		}
	}

	public void OnCollisionEnter(Collision c)
	{
		if (!disabled)
		{
			handleCollision(c.gameObject);
		}
	}

	private void handleCollision(GameObject c)
	{
		for (int i = 0; i < colliderLayers.Length; i++)
		{
			if (LayerMask.NameToLayer(colliderLayers[i]) == c.layer)
			{
				if (particleEffectPS != null && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
				{
					particleEffectPS.Emit(1);
					Object.Instantiate(blood, base.gameObject.transform.position, Quaternion.identity);
				}
				if (alwaysCutHead)
				{
					c.SendMessage("OnHeadChopped", SendMessageOptions.DontRequireReceiver);
				}
				if (isEnemyOwned)
				{
					c.SendMessage("OnHit", damage * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					c.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
				}
				SoundManager.Instance.playSound(hitSound);
				reduceAmmo();
			}
		}
	}

	private void reduceAmmo()
	{
		if (!unlimitedAmmo)
		{
			currentAmmo -= 1f;
			HUDController.Instance.addAmmo(weaponType, -1f);
		}
	}
}
