using UnityEngine;

public class TentacleDmg : MonoBehaviour
{
	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			c.gameObject.SendMessage("OnHit", 10f * GameManager.Instance.getDamageMultiplier(), SendMessageOptions.DontRequireReceiver);
		}
	}
}
