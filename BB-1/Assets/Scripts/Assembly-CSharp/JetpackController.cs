using UnityEngine;

public class JetpackController : MonoBehaviour
{
	private ParticleSystem emitterPS;

	public float damage = 40f;

	private void Awake()
	{
		emitterPS = GetComponent<ParticleSystem>();
	}

	public void OnTriggerEnter(Collider c)
	{
		if (emitterPS.enableEmission && c.CompareTag("Player"))
		{
			c.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
		}
	}
}
