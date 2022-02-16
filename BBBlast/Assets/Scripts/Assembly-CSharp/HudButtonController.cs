using System.Collections;
using UnityEngine;

public class HudButtonController : MonoBehaviour
{
	public GameObject press;

	private void Awake()
	{
		if (press != null)
		{
			//press.active = false;
			press.SetActive(false);
		}
	}

	private void Start()
	{
		if (press != null)
		{
			//press.active = false;
			press.SetActive(false);
		}
	}

	public void pressed()
	{
		if (press != null)
		{
			//press.active = true;
			press.SetActive(true);
		}
	}

	public void unpressed()
	{
		if (press != null)
		{
			//press.active = false;
			press.SetActive(false);
		}
	}

	public void clicked()
	{
		SendMessageUpwards("ButtonPress", base.gameObject.name);
		StartCoroutine("waitUnpress");
	}

	public IEnumerator waitUnpress()
	{
		yield return new WaitForSeconds(0f);
		unpressed();
	}
}
