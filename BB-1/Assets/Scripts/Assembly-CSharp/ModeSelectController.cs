using System.Collections;
using UnityEngine;

public class ModeSelectController : MonoBehaviour
{
	public GameObject campaignUp;

	public GameObject campaignPress;

	public GameObject survivalUp;

	public GameObject survivalPress;

	public GameObject bossTrialUp;

	public GameObject bossTrialPress;

	public GameObject BossTrials;

	public GameObject UnderContstruction;

	public GUIButton goButton;

	private void Start()
	{
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			BossTrials.SetActiveRecursively(false);
			UnderContstruction.SetActiveRecursively(true);
		}
		else
		{
			BossTrials.SetActiveRecursively(true);
			UnderContstruction.SetActiveRecursively(false);
		}
		GameManager.Instance.currentGameMode = GameMode.NONE;
		goButton.disable();
	}

	private IEnumerator delayedUpdateSelection()
	{
		yield return new WaitForSeconds(0.01f);
		updateSelection();
	}

	private void updateSelection()
	{
		switch (GameManager.Instance.currentGameMode)
		{
		case GameMode.NONE:
			goButton.disable();
			break;
		case GameMode.BOSSTRIAL:
			campaignUp.SetActiveRecursively(true);
			campaignPress.SetActiveRecursively(false);
			survivalUp.SetActiveRecursively(true);
			survivalPress.SetActiveRecursively(false);
			bossTrialUp.SetActiveRecursively(false);
			bossTrialPress.SetActiveRecursively(true);
			goButton.disable();
			break;
		case GameMode.CAMPAIGN:
			campaignUp.SetActiveRecursively(false);
			campaignPress.SetActiveRecursively(true);
			survivalUp.SetActiveRecursively(true);
			survivalPress.SetActiveRecursively(false);
			bossTrialUp.SetActiveRecursively(true);
			bossTrialPress.SetActiveRecursively(false);
			goButton.disable();
			break;
		case GameMode.SURVIVAL:
			campaignUp.SetActiveRecursively(true);
			campaignPress.SetActiveRecursively(false);
			survivalUp.SetActiveRecursively(false);
			survivalPress.SetActiveRecursively(true);
			bossTrialUp.SetActiveRecursively(true);
			bossTrialPress.SetActiveRecursively(false);
			goButton.enable();
			break;
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.gameObject.name)
		{
		case "bossTrials_btn":
			GameManager.Instance.currentGameMode = GameMode.BOSSTRIAL;
			updateSelection();
			SendMessageUpwards("OnGUIButtonClicked", goButton);
			break;
		case "Campaign_btn":
			GameManager.Instance.currentGameMode = GameMode.CAMPAIGN;
			updateSelection();
			SendMessageUpwards("OnGUIButtonClicked", goButton);
			break;
		case "Survival_btn":
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
				break;
			}
			GameManager.Instance.currentGameMode = GameMode.SURVIVAL;
			goButton.gameObject.SetActiveRecursively(true);
			updateSelection();
			break;
		}
	}
}
