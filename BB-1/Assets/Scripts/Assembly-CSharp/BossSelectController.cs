using System.Collections;
using UnityEngine;

public class BossSelectController : MonoBehaviour
{
	public GUIButton mechaButton;

	public GUIButton tentacleButton;

	public GUIButton spacebossButton;

	public GUIButton goButton;

	public GameObject mechaUp;

	public GameObject mechaPress;

	public GameObject tentacleUp;

	public GameObject tentaclePress;

	public GameObject spacebossUp;

	public GameObject spacebossPress;

	public GameObject underConstruction;

	public GameObject underConstruction2;

	public GameObject underConstruction3;

	private void Start()
	{
		GameManager.Instance.currentBossTrial = BossMode.NONE;
		mechaButton.disable();
		tentacleButton.disable();
		spacebossButton.disable();
		if (PlayerPrefs.HasKey("MechabossUnlocked") || Application.isEditor)
		{
			mechaButton.enable();
		}
		if (PlayerPrefs.HasKey("TentacleeseUnlocked") || Application.isEditor)
		{
			tentacleButton.enable();
		}
		if (GameManager.Instance.currentCharacter == Character.OLIVER && (PlayerPrefs.HasKey("SpacebossUnlocked") || Application.isEditor))
		{
			spacebossButton.enable();
		}
		if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			underConstruction.SetActiveRecursively(true);
			spacebossButton.gameObject.SetActiveRecursively(false);
		}
		else if (GameManager.Instance.currentCharacter == Character.OLIVER)
		{
			underConstruction.SetActiveRecursively(false);
		}
		else if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			mechaButton.gameObject.SetActiveRecursively(false);
			tentacleButton.gameObject.SetActiveRecursively(false);
			spacebossButton.gameObject.SetActiveRecursively(false);
		}
		goButton.disable();
	}

	private IEnumerator delayedUpdateSelection()
	{
		yield return new WaitForSeconds(0.01f);
		updateSelection();
	}

	private void updateSelection()
	{
		switch (GameManager.Instance.currentBossTrial)
		{
		case BossMode.NONE:
			goButton.disable();
			break;
		case BossMode.MECHABEARZERKER:
			if (!tentacleButton.isDisabled())
			{
				tentacleUp.SetActiveRecursively(true);
				tentaclePress.SetActiveRecursively(false);
			}
			if (!spacebossButton.isDisabled())
			{
				spacebossUp.SetActiveRecursively(true);
				spacebossPress.SetActiveRecursively(false);
			}
			mechaUp.SetActiveRecursively(false);
			mechaPress.SetActiveRecursively(true);
			goButton.enable();
			break;
		case BossMode.TENTACLEESE:
			if (!mechaButton.isDisabled())
			{
				mechaUp.SetActiveRecursively(true);
				mechaPress.SetActiveRecursively(false);
			}
			if (!spacebossButton.isDisabled())
			{
				spacebossUp.SetActiveRecursively(true);
				spacebossPress.SetActiveRecursively(false);
			}
			tentacleUp.SetActiveRecursively(false);
			tentaclePress.SetActiveRecursively(true);
			goButton.enable();
			break;
		case BossMode.SPACEBOSS:
			if (!mechaButton.isDisabled())
			{
				mechaUp.SetActiveRecursively(true);
				mechaPress.SetActiveRecursively(false);
			}
			if (!tentacleButton.isDisabled())
			{
				tentacleUp.SetActiveRecursively(true);
				tentaclePress.SetActiveRecursively(false);
			}
			spacebossUp.SetActiveRecursively(false);
			spacebossPress.SetActiveRecursively(true);
			goButton.enable();
			break;
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.gameObject.name)
		{
		case "mechabearzerker_btn":
			if (GameManager.Instance.currentBossTrial == BossMode.MECHABEARZERKER)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
			}
			else
			{
				GameManager.Instance.currentBossTrial = BossMode.MECHABEARZERKER;
			}
			break;
		case "tentacleese_btn":
			if (GameManager.Instance.currentBossTrial == BossMode.TENTACLEESE)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
			}
			else
			{
				GameManager.Instance.currentBossTrial = BossMode.TENTACLEESE;
			}
			break;
		case "spaceboss_btn":
			if (GameManager.Instance.currentBossTrial == BossMode.SPACEBOSS)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
			}
			else
			{
				GameManager.Instance.currentBossTrial = BossMode.SPACEBOSS;
			}
			break;
		}
		updateSelection();
	}
}
