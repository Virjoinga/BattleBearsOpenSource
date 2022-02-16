using UnityEngine;

public class SecretBodyPart : MonoBehaviour
{
	private float collisionDamage = 25f;

	public void OnHit(float damage)
	{
		SendMessageUpwards("OnSecretColliderHit", damage);
	}
}
