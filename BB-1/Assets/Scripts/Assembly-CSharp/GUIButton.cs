using UnityEngine;

public class GUIButton : MonoBehaviour
{
	public bool inActive;

	public GameObject listener;

	public GameObject pressObj;

	public GameObject upObj;

	public GameObject greyObj;

	private Transform myTransform;

	public bool disabled;

	public bool unpressWhenReleased;

	private void Awake()
	{
		myTransform = base.transform;
		if (pressObj == null)
		{
			foreach (Transform item in myTransform)
			{
				if (item.name.StartsWith("press"))
				{
					pressObj = item.gameObject;
				}
			}
		}
		if (upObj == null)
		{
			foreach (Transform item2 in myTransform)
			{
				if (item2.name.StartsWith("up"))
				{
					upObj = item2.gameObject;
				}
			}
		}
		if (greyObj == null)
		{
			foreach (Transform item3 in myTransform)
			{
				if (item3.name.StartsWith("grey"))
				{
					greyObj = item3.gameObject;
				}
			}
		}
		if (disabled)
		{
			disable();
		}
		else
		{
			enable();
		}
	}

	private void Start()
	{
		if (pressObj != null)
		{
			pressObj.SetActive(false);
		}
	}

	public bool isDisabled()
	{
		return disabled;
	}

	public void disable()
	{
		disabled = true;
		if (greyObj != null)
		{
			greyObj.SetActive(true);
		}
		if (upObj != null)
		{
			upObj.SetActive(false);
		}
		if (pressObj != null)
		{
			pressObj.SetActive(false);
		}
	}

	public void enable()
	{
		disabled = false;
		if (greyObj != null)
		{
			greyObj.SetActive(false);
		}
		if (upObj != null)
		{
			upObj.SetActive(true);
		}
		if (pressObj != null)
		{
			pressObj.SetActive(false);
		}
	}

	public void OnButtonPressed()
	{
		if (!disabled)
		{
			if (upObj != null)
			{
				upObj.SetActive(false);
			}
			if (pressObj != null)
			{
				pressObj.SetActive(true);
			}
		}
	}

	public void OnButtonDepressed()
	{
		if (!disabled)
		{
			Debug.Log("depressed");
			if (upObj != null)
			{
				upObj.SetActive(true);
			}
			if (pressObj != null)
			{
				pressObj.SetActive(false);
			}
		}
	}

	public void OnButtonClicked()
	{
		if (!disabled && !inActive)
		{
			SendMessageUpwards("OnGUIButtonClicked", this, SendMessageOptions.DontRequireReceiver);
			if (listener != null)
			{
				listener.SendMessage("OnGUIButtonClicked", this, SendMessageOptions.DontRequireReceiver);
			}
			if (unpressWhenReleased)
			{
				OnButtonDepressed();
			}
		}
	}
}
