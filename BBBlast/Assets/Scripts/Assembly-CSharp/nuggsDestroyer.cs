using UnityEngine;

public class nuggsDestroyer : MonoBehaviour
{
	public void OnTriggerEnter(Collider c)
	{
		c.gameObject.SendMessage("delete");
		GameManager.Instance.canHasNuggs = true;
	}
}
