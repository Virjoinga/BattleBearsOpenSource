using UnityEngine;

public class ProjectileControllerParticle : ProjectileController
{
	public float damage;

	protected ParticleSystem PEPS;

	public string[] ignoreTags = new string[0];

	protected override void Awake()
	{
		base.Awake();
		PEPS = GetComponent<ParticleSystem>();
	}

	public override void FireProjectile(Transform t, int numberOfBullets)
	{
		if (Time.timeScale > 0f)
		{
			float timeScale = Time.timeScale;
			float num = 1f;
		}
		PEPS.Emit(numberOfBullets);
	}

	private void OnParticleCollision(GameObject other)
	{
		bool flag = false;
		if (ignoreTags.Length != 0)
		{
			for (int i = 0; i < ignoreTags.Length; i++)
			{
				if (other.CompareTag(ignoreTags[i]))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			Vector3 normalized = (other.transform.position - myTransform.position).normalized;
			other.SendMessage("OnImpact", normalized, SendMessageOptions.DontRequireReceiver);
			other.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
		}
	}
}
