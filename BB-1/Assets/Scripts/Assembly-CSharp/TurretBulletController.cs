using UnityEngine;

public class TurretBulletController : MonoBehaviour
{
	public float damage = 10f;

	private Transform myTransform;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void OnParticleCollision(GameObject other)
	{
		Vector3 normalized = (other.transform.position - myTransform.position).normalized;
		other.SendMessage("OnImpact", normalized, SendMessageOptions.DontRequireReceiver);
		other.SendMessage("OnHit", damage * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
		if (other.layer == LayerMask.NameToLayer("PlayerDefence"))
		{
			other.SendMessage("OnBlock");
		}
	}
}
