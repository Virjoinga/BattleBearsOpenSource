using UnityEngine;

public class LighningDmg : MonoBehaviour
{
	public int damage;

	private void OnParticleCollision(GameObject other)
	{
		other.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
	}
}
