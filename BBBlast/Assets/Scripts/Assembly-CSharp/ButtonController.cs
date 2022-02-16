using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	public bool onlyOnce;

	public Material selectMat;

	private Material orgMat;

	public GameObject button;

	private bool disabled;

	private void Start()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			orgMat = base.GetComponent<Renderer>().material;
		}
		if (button != null)
		{
			button.active = false;
		}
		disabled = false;
	}

	public void selected()
	{
		if (selectMat != null)
		{
			base.GetComponent<Renderer>().material = selectMat;
		}
		if (button != null)
		{
			button.active = true;
		}
	}

	public void unSelected()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().material = orgMat;
		}
		if (button != null)
		{
			button.active = false;
		}
	}

	public void clicked()
	{
		if (onlyOnce)
		{
			if (!disabled)
			{
				SendMessageUpwards("ButtonPress", base.gameObject.name);
				disabled = true;
				StartCoroutine("downCount");
			}
		}
		else
		{
			SendMessageUpwards("ButtonPress", base.gameObject.name);
		}
	}

	public IEnumerator downCount()
	{
		yield return new WaitForSeconds(2f);
		disabled = false;
	}
}
