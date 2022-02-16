using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
	public string layerToHit;

	public float damagePerSecond = 100f;

	public void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer(layerToHit))
		{
			DamageReceiver damageReceiver = other.GetComponent(typeof(DamageReceiver)) as DamageReceiver;
			if (damageReceiver != null)
			{
				damageReceiver.OnHit(damagePerSecond * Time.deltaTime);
			}
		}
	}
}
