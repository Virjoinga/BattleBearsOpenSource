using System.Collections;
using UnityEngine;

public class DifficultySelectController : MonoBehaviour
{
	public GameObject easyUp;

	public GameObject easyPress;

	public GameObject mediumUp;

	public GameObject mediumPress;

	public GameObject hardUp;

	public GameObject hardPress;

	public GUIButton goButton;

	public GUIButton stageSelectButton;

	private bool goDisabled;

	private void Start()
	{
		GameManager.Instance.currentDifficulty = GameDifficulty.NONE;
		goButton.disable();
		if (GameManager.Instance.currentGameMode != 0)
		{
			return;
		}
		if (GameManager.Instance.currentCharacter == Character.OLIVER)
		{
			if (GameManager.Instance.oliverUnlockedStage > 0)
			{
				goDisabled = true;
			}
		}
		else if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			if (GameManager.Instance.riggsUnlockedStage > 0)
			{
				goDisabled = true;
			}
		}
		else if (GameManager.Instance.currentCharacter == Character.WIL && GameManager.Instance.wilUnlockedStage > 0)
		{
			goDisabled = true;
		}
	}

	private IEnumerator delayedUpdateSelection()
	{
		yield return new WaitForSeconds(0.01f);
		updateSelection();
		yield return new WaitForSeconds(0.01f);
	}

	private void updateSelection()
	{
		switch (GameManager.Instance.currentDifficulty)
		{
		case GameDifficulty.NONE:
			goButton.disable();
			break;
		case GameDifficulty.EASY:
			hardUp.SetActiveRecursively(true);
			hardPress.SetActiveRecursively(false);
			mediumUp.SetActiveRecursively(true);
			mediumPress.SetActiveRecursively(false);
			easyUp.SetActiveRecursively(false);
			easyPress.SetActiveRecursively(true);
			if (!goDisabled)
			{
				goButton.enable();
			}
			break;
		case GameDifficulty.MEDIUM:
			hardUp.SetActiveRecursively(true);
			hardPress.SetActiveRecursively(false);
			mediumUp.SetActiveRecursively(false);
			mediumPress.SetActiveRecursively(true);
			easyUp.SetActiveRecursively(true);
			easyPress.SetActiveRecursively(false);
			if (!goDisabled)
			{
				goButton.enable();
			}
			break;
		case GameDifficulty.HARD:
			hardUp.SetActiveRecursively(false);
			hardPress.SetActiveRecursively(true);
			mediumUp.SetActiveRecursively(true);
			mediumPress.SetActiveRecursively(false);
			easyUp.SetActiveRecursively(true);
			easyPress.SetActiveRecursively(false);
			if (!goDisabled)
			{
				goButton.enable();
			}
			break;
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.gameObject.name)
		{
		case "easy_btn":
			if (!goDisabled)
			{
				if (GameManager.Instance.currentDifficulty == GameDifficulty.EASY)
				{
					GameManager.Instance.startingStage = 1;
					SendMessageUpwards("OnGUIButtonClicked", goButton);
				}
				else
				{
					GameManager.Instance.currentDifficulty = GameDifficulty.EASY;
					updateSelection();
				}
			}
			else
			{
				GameManager.Instance.currentDifficulty = GameDifficulty.EASY;
				updateSelection();
				SendMessageUpwards("OnGUIButtonClicked", stageSelectButton);
			}
			break;
		case "medium_btn":
			if (!goDisabled)
			{
				if (GameManager.Instance.currentDifficulty == GameDifficulty.MEDIUM)
				{
					GameManager.Instance.startingStage = 1;
					SendMessageUpwards("OnGUIButtonClicked", goButton);
				}
				else
				{
					GameManager.Instance.currentDifficulty = GameDifficulty.MEDIUM;
					updateSelection();
				}
			}
			else
			{
				GameManager.Instance.currentDifficulty = GameDifficulty.MEDIUM;
				updateSelection();
				SendMessageUpwards("OnGUIButtonClicked", stageSelectButton);
			}
			Debug.Log(GameManager.Instance.currentDifficulty);
			break;
		case "hard_btn":
			if (!goDisabled)
			{
				if (GameManager.Instance.currentDifficulty == GameDifficulty.HARD)
				{
					GameManager.Instance.startingStage = 1;
					SendMessageUpwards("OnGUIButtonClicked", goButton);
				}
				else
				{
					GameManager.Instance.currentDifficulty = GameDifficulty.HARD;
					updateSelection();
				}
			}
			else
			{
				GameManager.Instance.currentDifficulty = GameDifficulty.HARD;
				updateSelection();
				SendMessageUpwards("OnGUIButtonClicked", stageSelectButton);
			}
			Debug.Log(GameManager.Instance.currentDifficulty);
			break;
		}
	}
}
