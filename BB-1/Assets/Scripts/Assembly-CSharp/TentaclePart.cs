using UnityEngine;

public class TentaclePart : MonoBehaviour
{
	private float collisionDamage = 25f;

	public GameObject emitter;

	public void OnSinkingIntoGround()
	{
		if (emitter != null)
		{
			GameObject obj = Object.Instantiate(emitter);
			obj.transform.parent = base.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = Vector3.zero;
		}
	}

	public void OnHit(float damage)
	{
		SendMessageUpwards("OnTentaclePartHit", damage);
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			c.gameObject.SendMessage("OnHit", collisionDamage * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
		}
	}
}
