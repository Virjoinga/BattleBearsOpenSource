using UnityEngine;

public class DieNoPoints : MonoBehaviour
{
	public void OnTriggerStay(Collider c)
	{
		c.gameObject.SendMessage("DieNoPnts", SendMessageOptions.DontRequireReceiver);
	}

	public void OnTriggerEnter(Collider c)
	{
		c.gameObject.SendMessage("DieNoPnts", SendMessageOptions.DontRequireReceiver);
	}
}
