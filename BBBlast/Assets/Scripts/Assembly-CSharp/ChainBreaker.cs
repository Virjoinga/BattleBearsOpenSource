using UnityEngine;

public class ChainBreaker : MonoBehaviour
{
	public void swell()
	{
		GameObject.Find("enemies").BroadcastMessage("broken");
	}
}
