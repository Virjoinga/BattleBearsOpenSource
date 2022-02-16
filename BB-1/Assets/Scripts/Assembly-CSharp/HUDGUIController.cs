using BSCore;
using UnityEngine;

public class HUDGUIController : MonoBehaviour
{
	public static bool isActive = true;

	public Camera guiCamera;

	public LayerMask layerMask;

	public int selection;

	private GameObject OCOHUD;

	private void OnLevelWasLoaded()
	{
		selection = 3;
		OCOHUD = GameObject.Find("OCO_HUD");
	}

	public void Update()
	{
		if (!isActive)
		{
			return;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			selection++;
			if (selection > 3)
			{
				selection = 0;
			}
		}
		if (BSCoreInput.GetButtonDown(Option.WeaponSlot1))
		{
			selection = ((selection == 0) ? 3 : 0);
		}
		if (BSCoreInput.GetButtonDown(Option.WeaponSlot2))
		{
			selection = ((selection != 1) ? 1 : 3);
		}
		if (BSCoreInput.GetButtonDown(Option.WeaponSlot3))
		{
			selection = ((selection == 2) ? 3 : 2);
		}
		if (OCOHUD != null)
		{
			OCOHUD.GetComponent<OCOHUDController>().updateSelection(selection);
		}
	}
}
