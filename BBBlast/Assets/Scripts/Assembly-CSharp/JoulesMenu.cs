using UnityEngine;

public class JoulesMenu : MonoBehaviour
{
	private InAppPlugin iap;

	private void Start()
	{
		iap = GameObject.Find("inAppPlugin").GetComponent<InAppPlugin>();
	}

	private void Update()
	{
	}

	public void ButtonPress(string name)
	{
		if (name.Equals("Cancel"))
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			iap.BuyJoules(name);
		}
	}
}
