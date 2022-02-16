using UnityEngine;

public class ExplosionController : MonoBehaviour
{
	public Enemy myColor;

	public void OnTriggerEnter(Collider c)
	{
		c.gameObject.SendMessage("colorHit", myColor, SendMessageOptions.DontRequireReceiver);
	}
}
