using UnityEngine;

public class UdderBody : MonoBehaviour
{
	private float collisionDamage = 50f;

	public void OnHit(float damage)
	{
		SendMessageUpwards("OnUdderHit", damage, SendMessageOptions.DontRequireReceiver);
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			c.gameObject.SendMessage("OnHit", collisionDamage * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.tag == "Player")
		{
			c.gameObject.SendMessage("OnHit", collisionDamage * GameManager.Instance.getDamageMultiplier() * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
		}
	}
}
