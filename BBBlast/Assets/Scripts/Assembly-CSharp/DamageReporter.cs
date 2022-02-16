using UnityEngine;

public class DamageReporter : MonoBehaviour
{
	public GameObject sendTo;

	private void OnHit(float dmg)
	{
		sendTo.SendMessage("gotHit", dmg);
	}

	private void delete()
	{
		sendTo.SendMessage("delete", SendMessageOptions.DontRequireReceiver);
	}

	public void explode()
	{
		sendTo.SendMessage("explode", SendMessageOptions.DontRequireReceiver);
	}
}
