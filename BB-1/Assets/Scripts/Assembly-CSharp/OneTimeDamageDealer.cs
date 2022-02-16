using System.Collections;
using UnityEngine;

public class OneTimeDamageDealer : MonoBehaviour
{
	public float damage = 80f;

	public bool explosion;

	public string[] colliderLayers;

	public bool isEnemyOwned;

	public bool disabled;

	public float shakeCameraTime;

	private void Awake()
	{
		StartCoroutine(delayedDisable());
	}

	private IEnumerator delayedDisable()
	{
		yield return new WaitForSeconds(0.05f);
		disabled = true;
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
			if (LayerMask.NameToLayer(colliderLayers[i]) != c.layer)
			{
				continue;
			}
			if (explosion)
			{
				c.SendMessage("OnHitByExplosion", SendMessageOptions.DontRequireReceiver);
			}
			if (isEnemyOwned)
			{
				c.SendMessage("OnHit", damage * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
				if (shakeCameraTime > 0f)
				{
					c.transform.root.BroadcastMessage("OnShake", shakeCameraTime, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				c.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
			}
			break;
		}
	}
}
