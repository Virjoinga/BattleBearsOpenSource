using UnityEngine;

public class GUIController : MonoBehaviour
{
	public static bool isActive = true;

	public Camera guiCamera;

	public LayerMask layerMask;

	private GameObject currentlySelectedButton;

	private Controlls controlls;

	private bool touch;

	private void Start()
	{
		controlls = GameObject.Find("Controlls").GetComponent<Controlls>();
		if (guiCamera == null)
		{
			Transform transform = base.transform.root.Find("Camera");
			if (transform != null)
			{
				guiCamera = transform.GetComponent<Camera>();
			}
			else
			{
				guiCamera = GameObject.Find("MenuCamera").GetComponent<Camera>();
			}
		}
	}

	public void Update()
	{
		if (!isActive)
		{
			return;
		}
		if (controlls.comp)
		{
			touch = true;
		}
		else if (Input.touches.Length != 0)
		{
			touch = true;
		}
		else
		{
			touch = false;
		}
		if (!touch)
		{
			return;
		}
		controlls.began();
		controlls.ended();
		controlls.moved();
		if (!controlls.began() && !controlls.ended() && !controlls.moved())
		{
			return;
		}
		Ray ray = guiCamera.ScreenPointToRay(controlls.pos());
		RaycastHit hitInfo;
		if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, 1000f, layerMask))
		{
			GameObject gameObject = hitInfo.transform.gameObject;
			if (currentlySelectedButton == null && controlls.began())
			{
				currentlySelectedButton = gameObject;
				gameObject.SendMessage("OnButtonPressed", SendMessageOptions.DontRequireReceiver);
			}
			else if (currentlySelectedButton != null && controlls.ended())
			{
				if (gameObject == currentlySelectedButton)
				{
					currentlySelectedButton = null;
					gameObject.SendMessage("OnButtonClicked", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					currentlySelectedButton.SendMessage("OnButtonDepressed", SendMessageOptions.DontRequireReceiver);
					currentlySelectedButton = null;
				}
			}
			else if (gameObject == currentlySelectedButton)
			{
				gameObject.SendMessage("OnButtonPressed", SendMessageOptions.DontRequireReceiver);
			}
			else if (currentlySelectedButton != null)
			{
				currentlySelectedButton.SendMessage("OnButtonDepressed", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (currentlySelectedButton != null)
		{
			currentlySelectedButton.SendMessage("OnButtonDepressed", SendMessageOptions.DontRequireReceiver);
		}
	}
}
